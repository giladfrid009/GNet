using GNet.Layers;
using System;
using System.Collections.Generic;

namespace GNet.ComputaionGraph
{
    [Serializable]
    public class Node
    {
        public ImmutableArray<Node> InNodes { get; }
        public ImmutableArray<Node> OutNodes { get; private set; }
        public ImmutableArray<Layer> Layers { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }
        public int Length { get; }

        private bool hasProcessed = false;

        [NonSerialized]
        private readonly List<Node> listOutNodes;

        public Node(ImmutableArray<Node> inNodes, ImmutableArray<Layer> layers, bool initLayers = true)
        {
            InNodes = inNodes;
            OutNodes = new ImmutableArray<Node>();
            Layers = layers;
            Length = layers.Length;
            InputShape = layers[0].Shape;
            OutputShape = layers[^1].Shape;
            listOutNodes = new List<Node>();

            inNodes.ForEach(N => N.listOutNodes.Add(this));

            if (initLayers)
            {
                Connect();
                Initialize();
            }
        }

        public Node(ImmutableArray<Node> inNodes, params Layer[] layers) : this(inNodes, new ImmutableArray<Layer>(layers))
        {
        }

        public Node(ImmutableArray<Layer> layers, bool initLayers = true) : this(new ImmutableArray<Node>(), layers, initLayers)
        {
        }

        public Node(params Layer[] layers) : this(new ImmutableArray<Node>(), new ImmutableArray<Layer>(layers))
        {
        }

        public void Interconnect()
        {
            if (hasProcessed)
            {
                return;
            }

            hasProcessed = true;

            if (InNodes.Length > 0)
            {
                if ((Layers[0] is MergeLayer) == false)
                {
                    throw new ArgumentException($"{nameof(Layers)}[0] is not of type {nameof(MergeLayer)}");
                }

                ((MergeLayer)Layers[0]).Connect(InNodes.Select(N => N.Layers[^1]));

                Layers[0].Initialize();
            }

            OutNodes = ImmutableArray<Node>.FromRef(listOutNodes.ToArray());
            listOutNodes.Clear();       

            OutNodes.ForEach(N => N.Interconnect());
        }

        public void ResetProcessed()
        {
            if (hasProcessed == false)
            {
                return;
            }

            hasProcessed = false;

            OutNodes.ForEach(N => N.ResetProcessed());
        }

        private void Connect()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Connect(Layers[i - 1]);
            }
        }

        private void Initialize()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Initialize();
            }
        }      

        public void Forward(ImmutableShapedArray<double> inputs, bool isTraining)
        {
            if (hasProcessed)
            {
                return;
            }

            hasProcessed = true;

            Layers[0].Input(inputs);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Forward(isTraining);
            }

            OutNodes.ForEach(N => N.Forward(isTraining));
        }

        private void Forward(bool isTraining)
        {
            if (hasProcessed)
            {
                return;
            }

            InNodes.ForEach(N =>
            {
                if (N.hasProcessed == false)
                {
                    return;
                }
            });

            hasProcessed = true;

            Layers.ForEach(L => L.Forward(isTraining));
            OutNodes.ForEach(N => N.Forward(isTraining));
        }

        public void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (hasProcessed)
            {
                return;
            }

            hasProcessed = true;

            Layers[Length - 1].CalcGrads(loss, targets);

            for (int i = Length - 2; i >= 0; i--)
            {
                Layers[i].CalcGrads();
            }

            InNodes.ForEach(N => N.CalcGrads());
        }

        private void CalcGrads()
        {
            if (hasProcessed)
            {
                return;
            }

            OutNodes.ForEach(N =>
            {
                if (N.hasProcessed == false)
                {
                    return;
                }
            });

            hasProcessed = true;
            
            for (int i = Length - 1; i >= 0; i--)
            {
                Layers[i].CalcGrads();
            }

            InNodes.ForEach(N => N.CalcGrads());
        }

        public void Optimize(IOptimizer optimizer)
        {
            if (hasProcessed)
            {
                return;
            }

            hasProcessed = true;

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Optimize(optimizer);
            }

            OutNodes.ForEach(N => N.Optimize(optimizer));
        }

        public void Update()
        {
            if (hasProcessed)
            {
                return;
            }

            hasProcessed = true;

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Update();
            }

            OutNodes.ForEach(N => N.Update());
        }

        public void ClearCache()
        {
            if (hasProcessed)
            {
                return;
            }

            hasProcessed = true;

            Layers.ForEach(L => L.Neurons.ForEach(N =>
            {
                N.ClearCache();
                N.InSynapses.ForEach(S => S.ClearCache());
            }));

            OutNodes.ForEach(N => N.ClearCache());
        }
    }
}