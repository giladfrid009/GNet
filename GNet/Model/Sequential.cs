using System;

namespace GNet
{
    [Serializable]
    public class Sequential : Network
    {
        public ImmutableArray<Layer> Layers { get; }        
        public int Length { get; }

        public Sequential(ImmutableArray<Layer> layers) : base(layers[0].Shape, layers[^1].Shape)
        {
            Layers = layers;
            Length = layers.Length;            

            Connect();
            Initialize();
        }

        public Sequential(params Layer[] layers) : this(new ImmutableArray<Layer>(layers))
        {
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

        protected override void Forward(ImmutableShapedArray<double> inputs, bool isTraining)
        {
            Layers[0].Input(inputs);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Forward(isTraining);
            }
        }

        protected override void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            Layers[Length - 1].CalcGrads(loss, targets);

            for (int i = Length - 2; i > 0; i--)
            {
                Layers[i].CalcGrads();
            }
        }

        protected override void Optimize(IOptimizer optimizer)
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Optimize(optimizer);
            }
        }

        protected override void Update()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Update();
            }
        }

        protected override void ClearCache()
        {
            Layers.ForEach(L => L.Neurons.ForEach(N =>
            {
                N.ClearCache();
                N.InSynapses.ForEach(S => S.ClearCache());
            }));
        }

        protected override ImmutableShapedArray<double> GetOutput()
        {
            return Layers[Length - 1].Neurons.Select(N => N.OutVal).ToShape(OutputShape);
        }
    }
}