using GNet.Layers;
using NCollections;
using System;
using System.Collections.Generic;

namespace GNet.CompGraph
{
    [Serializable]
    public class Node : Sequential
    {
        private enum Ops { None, Connect, Forward, CalcGrads, Optimize, Update, ClearCache }

        public Array<Node> InNodes { get; }
        public Array<Node> OutNodes { get; private set; }

        [NonSerialized]
        private readonly List<Node> listOutNodes;

        private Ops lastOp = Ops.None;

        public Node(Array<Node> inNodes, Array<Layer> layers) : base(layers)
        {
            InNodes = inNodes;
            OutNodes = new Array<Node>();
            listOutNodes = new List<Node>();

            inNodes.ForEach(N => N.listOutNodes.Add(this));
        }

        public Node(Array<Node> inNodes, params Layer[] layers) : this(inNodes, new Array<Layer>(layers))
        {
        }

        public Node(Array<Layer> layers) : this(new Array<Node>(), layers)
        {
        }

        public Node(params Layer[] layers) : this(new Array<Node>(), new Array<Layer>(layers))
        {
        }

        private void ResetOps()
        {
            if (lastOp == Ops.None)
            {
                return;
            }

            lastOp = Ops.None;

            OutNodes.ForEach(N => N.ResetOps());
        }

        private void Forward(bool isTraining)
        {
            if (lastOp == Ops.Forward)
            {
                return;
            }

            InNodes.ForEach(N =>
            {
                if (N.lastOp != Ops.Forward)
                {
                    return;
                }
            });

            lastOp = Ops.Forward;

            Layers.ForEach(L => L.Forward(isTraining));
            OutNodes.ForEach(N => N.Forward(isTraining));
        }

        private void CalcGrads()
        {
            if (lastOp == Ops.CalcGrads)
            {
                return;
            }

            OutNodes.ForEach(N =>
            {
                if (N.lastOp != Ops.CalcGrads)
                {
                    return;
                }
            });

            lastOp = Ops.CalcGrads;

            for (int i = Length - 1; i >= 0; i--)
            {
                Layers[i].CalcGrads();
            }

            InNodes.ForEach(N => N.CalcGrads());
        }

        public void Connect()
        {
            if (lastOp == Ops.Connect)
            {
                return;
            }

            lastOp = Ops.Connect;

            if (InNodes.Length > 0)
            {
                if ((Layers[0] is MergeLayer) == false)
                {
                    throw new ArgumentException($"{nameof(Layers)}[0] is not of type {nameof(MergeLayer)}");
                }

                ((MergeLayer)Layers[0]).Connect(InNodes.Select(N => N.Layers[^1]));

                Layers[0].Initialize();
            }

            OutNodes = Array<Node>.FromRef(listOutNodes.ToArray());
            listOutNodes.Clear();

            OutNodes.ForEach(N => N.Connect());
        }

        public new void Forward(Tensor<double> inputs, bool isTraining)
        {
            ResetOps();

            lastOp = Ops.Forward;

            base.Forward(inputs, isTraining);

            OutNodes.ForEach(N => N.Forward(isTraining));
        }

        public new void CalcGrads(ILoss loss, Tensor<double> targets)
        {
            lastOp = Ops.CalcGrads;

            base.CalcGrads(loss, targets);

            Layers[0].CalcGrads();

            InNodes.ForEach(N => N.CalcGrads());
        }

        public new void Optimize(IOptimizer optimizer)
        {
            if (lastOp == Ops.Optimize)
            {
                return;
            }

            lastOp = Ops.Optimize;

            base.Optimize(optimizer);

            OutNodes.ForEach(N => N.Optimize(optimizer));
        }

        public new void Update()
        {
            if (lastOp == Ops.Update)
            {
                return;
            }

            lastOp = Ops.Update;

            base.Update();

            OutNodes.ForEach(N => N.Update());
        }

        public new void ClearCache()
        {
            if (lastOp == Ops.ClearCache)
            {
                return;
            }

            lastOp = Ops.ClearCache;

            base.ClearCache();

            OutNodes.ForEach(N => N.ClearCache());
        }
    }
}