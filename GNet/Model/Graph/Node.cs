using GNet.Layers;
using System;

//todo: implement connecting
namespace GNet.CompGraph
{
    [Serializable]
    public class Node
    {
        public ImmutableArray<Node> InNodes { get; }
        public ImmutableArray<Node> OutNodes { get; set; }
        public ImmutableArray<Layer> Layers { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }
        public int Length { get; }

        public Node(ImmutableArray<Layer> layers)
        {
            InNodes = new ImmutableArray<Node>();
            OutNodes = new ImmutableArray<Node>();
            Layers = layers;    
            Length = layers.Length;
            InputShape = layers[0].Shape;
            OutputShape = layers[layers.Length - 1].Shape;

            Connect();
            Initialize(false);
        }

        public Node(params Layer[] layers) : this(new ImmutableArray<Layer>(layers))
        {
        }

        public Node(ImmutableArray<Node> inNodes, ImmutableArray<Layer> layers)
        {
            if((layers[0] is MergeLayer) == false)
            {
                throw new ArgumentException($"nameof{layers}[0] is not of type {nameof(MergeLayer)}");
            }

            InNodes = inNodes;
            OutNodes = new ImmutableArray<Node>();
            Layers = layers;
            Length = layers.Length;
            InputShape = layers[0].Shape;
            OutputShape = layers[layers.Length - 1].Shape;

            Connect(inNodes);
            Initialize(true);
        }

        private void Connect()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Connect(Layers[i - 1]);
            }
        }

        private void Connect(ImmutableArray<Node> inNodes)
        {
            ((MergeLayer)Layers[0]).Connect(inNodes.Select(N => N.Layers[N.Length - 1]));

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

        public void Forward(ImmutableShapedArray<double> inputs, bool isTraining)
        {
            Layers[0].Input(inputs);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Forward(isTraining);
            }

            OutNodes.ForEach(N => N.Forward(isTraining));
        }

        private void Forward(bool isTraining)
        {
            Layers.ForEach(L => L.Forward(isTraining));
            OutNodes.ForEach(N => N.Forward(isTraining));
        }

        public void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            Layers[Length - 1].CalcGrads(loss, targets);

            for (int i = Length - 2; i > 0; i--)
            {
                Layers[i].CalcGrads();
            }

            InNodes.ForEach(N => N.CalcGrads());
        }

        private void CalcGrads()
        {
            for (int i = Length - 1; i >= 0; i--)
            {
                Layers[i].CalcGrads();
            }

            InNodes.ForEach(N => N.CalcGrads());
        }

        public void Optimize(IOptimizer optimizer, int epoch)
        {
            optimizer.UpdateParams(epoch);
            Optimize(optimizer);
        }

        private void Optimize(IOptimizer optimizer)
        {
            Layers.ForEach(L => L.Optimize(optimizer));
            OutNodes.ForEach(N => N.Optimize(optimizer));
        }

        public void Update()
        {
            Layers.ForEach(L => L.Update());
            OutNodes.ForEach(N => N.Update());
        }

        public void ClearCache()
        {
            Layers.ForEach(L => L.Neurons.ForEach(N =>
            {
                N.ClearCache();
                N.InSynapses.ForEach(S => S.ClearCache());
            }));

            OutNodes.ForEach(N => N.ClearCache());
        }        
    }
}