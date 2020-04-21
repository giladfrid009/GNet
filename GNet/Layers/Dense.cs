using System;
using GNet.Model;

namespace GNet.Layers
{
    [Serializable]
    public class Dense : TrainableLayer
    {
        public override ImmutableShapedArray<Neuron> Neurons { get; }
        public override Shape Shape { get; }

        public Dense(Shape shape, IActivation activation, IInitializer? weightInit = null, IInitializer? biasInit = null) : base(activation, weightInit, biasInit)
        {
            Shape = shape;
            Neurons = new ImmutableShapedArray<Neuron>(shape, () => new Neuron());
        }

        public override void Connect(ILayer inLayer)
        {
            Neurons.ForEach(N => N.InSynapses = inLayer.Neurons.Select(inN => new Synapse(inN, N)));

            inLayer.Neurons.ForEach((inN, i) => inN.OutSynapses = Neurons.Select(outN => outN.InSynapses[i]));
        }

        public override void Initialize()
        {
            int inLength = Neurons[0].InSynapses.Length;
            int outLength = Shape.Volume;

            Neurons.ForEach(N =>
            {
                N.Bias = BiasInit.Initialize(inLength, outLength);
                N.InSynapses.ForEach(S => S.Weight = WeightInit.Initialize(inLength, outLength));
            });
        }

        public override void Input(ImmutableShapedArray<double> values)
        {
            if (values.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(values));
            }

            Neurons.ForEach((N, i) => N.InVal = values[i]);

            ImmutableArray<double> activated = Activation.Activate(Neurons.Select(N => N.InVal));

            Neurons.ForEach((N, i) => N.OutVal = activated[i]);
        }
    }
}