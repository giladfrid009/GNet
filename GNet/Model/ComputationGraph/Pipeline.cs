using GNet.Layers;
using System;
using System.Collections.Generic;

namespace GNet.ComputaionGraph
{
    [Serializable]
    public class Pipeline
    {
        public ImmutableArray<Pipeline> InPipes { get; }
        public ImmutableArray<Pipeline> OutPipes { get; private set; }
        public ImmutableArray<Layer> Layers { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }
        public int Length { get; }

        private bool hasProcessed = false;

        [NonSerialized]
        private readonly List<Pipeline> outPipesList;

        public Pipeline(ImmutableArray<Layer> layers)
        {
            InPipes = new ImmutableArray<Pipeline>();
            OutPipes = new ImmutableArray<Pipeline>();
            Layers = layers;    
            Length = layers.Length;
            InputShape = layers[0].Shape;
            OutputShape = layers[^1].Shape;
            outPipesList = new List<Pipeline>();

            Connect();
            Initialize(false);
        }

        public Pipeline(params Layer[] layers) : this(new ImmutableArray<Layer>(layers))
        {
        }

        public Pipeline(ImmutableArray<Pipeline> inPipes, ImmutableArray<Layer> layers)
        {
            if((layers[0] is MergeLayer) == false)
            {
                throw new ArgumentException($"nameof{layers}[0] is not of type {nameof(MergeLayer)}");
            }

            InPipes = inPipes;
            OutPipes = new ImmutableArray<Pipeline>();
            Layers = layers;
            Length = layers.Length;
            InputShape = layers[0].Shape;
            OutputShape = layers[^1].Shape;
            outPipesList = new List<Pipeline>();

            inPipes.ForEach(P => P.outPipesList.Add(this));

            Connect(inPipes);
            Initialize(true);
        }

        public Pipeline(ImmutableArray<Pipeline> inPipes, params Layer[] layers) : this(inPipes, new ImmutableArray<Layer>(layers))
        {
        }

        private void Connect(ImmutableArray<Pipeline> inPipes)
        {
            ((MergeLayer)Layers[0]).Connect(inPipes.Select(P => P.Layers[^1]));

            Connect();
        }

        private void Connect()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Connect(Layers[i - 1]);
            }
        }

        private void Initialize(bool initFirst)
        {
            int i = initFirst ? 0 : 1;

            for (; i < Length; i++)
            {
                Layers[i].Initialize();
            }
        }

        public void ResetProcessed()
        {
            if(hasProcessed == false)
                return;

            hasProcessed = false;

            OutPipes.ForEach(P => P.ResetProcessed());
        }

        public void InitOutNodes()
        {
            if (hasProcessed)
                return;

            hasProcessed = true;  

            OutPipes = ImmutableArray<Pipeline>.FromRef(outPipesList.ToArray());
            outPipesList.Clear();

            OutPipes.ForEach(P => P.InitOutNodes());
        }

        public void Forward(ImmutableShapedArray<double> inputs, bool isTraining)
        {
            if (hasProcessed)
                return;

            hasProcessed = true;

            Layers[0].Input(inputs);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Forward(isTraining);
            }

            OutPipes.ForEach(P => P.Forward(isTraining));
        }

        private void Forward(bool isTraining)
        {
            if (hasProcessed)
                return;

            InPipes.ForEach(P =>
            {
                if (P.hasProcessed == false)
                {
                    return;
                }
            });

            hasProcessed = true;

            Layers.ForEach(L => L.Forward(isTraining));
            OutPipes.ForEach(P => P.Forward(isTraining));
        }

        public void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (hasProcessed)
                return;

            hasProcessed = true;

            Layers[Length - 1].CalcGrads(loss, targets);

            for (int i = Length - 2; i >= 0; i--)
            {
                Layers[i].CalcGrads();
            }

            InPipes.ForEach(P => P.CalcGrads());
        }

        private void CalcGrads()
        {
            if (hasProcessed)
                return;

            OutPipes.ForEach(P =>
            {
                if (P.hasProcessed == false)
                {
                    return;
                }
            });

            hasProcessed = true;
            
            for (int i = Length - 1; i >= 0; i--)
            {
                Layers[i].CalcGrads();
            }

            InPipes.ForEach(P => P.CalcGrads());
        }

        public void Optimize(IOptimizer optimizer)
        {
            if (hasProcessed)
                return;

            hasProcessed = true;

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Optimize(optimizer);
            }

            OutPipes.ForEach(P => P.Optimize(optimizer));
        }

        public void Update()
        {
            if (hasProcessed)
                return;

            hasProcessed = true;

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Update();
            }

            OutPipes.ForEach(P => P.Update());
        }

        public void ClearCache()
        {
            if (hasProcessed)
                return;

            hasProcessed = true;

            Layers.ForEach(L => L.Neurons.ForEach(N =>
            {
                N.ClearCache();
                N.InSynapses.ForEach(S => S.ClearCache());
            }));

            OutPipes.ForEach(P => P.ClearCache());
        }        
    }
}