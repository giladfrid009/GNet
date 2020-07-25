using GNet.Layers;
using System;
using System.Collections.Generic;

namespace GNet.ComputaionGraph
{
    [Serializable]
    public class Node : Sequential
    {
        public ImmutableArray<Node> InNodes { get; }
        public ImmutableArray<Node> OutNodes { get; private set; }

        private bool hasProcessed = false;

        [NonSerialized]
        private readonly List<Node> listOutNodes;

        public Node(ImmutableArray<Node> inNodes, ImmutableArray<Layer> layers, bool initLayers = true) : base(layers, initLayers)
        {
            InNodes = inNodes;
            OutNodes = new ImmutableArray<Node>();
            listOutNodes = new List<Node>();

            inNodes.ForEach(N => N.listOutNodes.Add(this));
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

        public new void Forward(ImmutableShapedArray<double> inputs, bool isTraining)
        {
            if (hasProcessed)
            {
                return;
            }

            hasProcessed = true;

            base.Forward(inputs, isTraining);

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

        public new void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (hasProcessed)
            {
                return;
            }

            hasProcessed = true;

            base.CalcGrads(loss, targets);

            Layers[0].CalcGrads();

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

        public new void Optimize(IOptimizer optimizer)
        {
            if (hasProcessed)
            {
                return;
            }

            hasProcessed = true;

            base.Optimize(optimizer);

            OutNodes.ForEach(N => N.Optimize(optimizer));
        }

        public new void Update()
        {
            if (hasProcessed)
            {
                return;
            }

            hasProcessed = true;

            base.Update();

            OutNodes.ForEach(N => N.Update());
        }

        public new void ClearCache()
        {
            if (hasProcessed)
            {
                return;
            }

            hasProcessed = true;

            base.ClearCache();

            OutNodes.ForEach(N => N.ClearCache());
        }
    }
}