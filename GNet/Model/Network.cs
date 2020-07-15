using System;

namespace GNet
{
    [Serializable]
    public class Network : BaseNetwork
    {
        public ImmutableArray<Layer> Layers { get; }        
        public int Length { get; }

        public Network(ImmutableArray<Layer> layers) : base(layers[0].Shape, layers[^1].Shape)
        {
            Layers = layers;
            Length = layers.Length;            

            Connect();
            Initialize();
        }

        public Network(params Layer[] layers) : this(new ImmutableArray<Layer>(layers))
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

        protected sealed override void Forward(ImmutableShapedArray<double> inputs, bool isTraining)
        {
            Layers[0].Input(inputs);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Forward(isTraining);
            }
        }

        protected sealed override void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            Layers[Length - 1].CalcGrads(loss, targets);

            for (int i = Length - 2; i > 0; i--)
            {
                Layers[i].CalcGrads();
            }
        }

        protected sealed override void Optimize(IOptimizer optimizer, int epoch)
        {
            optimizer.UpdateParams(epoch);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Optimize(optimizer);
            }
        }

        protected sealed override void Update()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Update();
            }
        }

        protected sealed override void ClearCache()
        {
            Layers.ForEach(L => L.Neurons.ForEach(N =>
            {
                N.ClearCache();
                N.InSynapses.ForEach(S => S.ClearCache());
            }));
        }

        public sealed override ImmutableShapedArray<double> Predict(ImmutableShapedArray<double> inputs)
        {
            Forward(inputs, false);

            return Layers[Length - 1].Neurons.Select(N => N.OutVal).ToShape(OutputShape);
        }
    }
}