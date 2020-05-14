using GNet.Model;
using System;

namespace GNet.Layers
{
    [Serializable]
    public class Dense : TrainableLayer
    {
        public override ImmutableArray<Neuron> Neurons { get; }

        public Dense(Shape shape, IActivation activation, IInitializer? weightInit = null, IInitializer? biasInit = null) : base(shape, activation, weightInit, biasInit)
        {
            Neurons = new ImmutableArray<Neuron>(shape.Volume, () => new Neuron());
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

        public override void Input(ImmutableShapedArray<double> values, bool isTraining)
        {
            if (values.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(values));
            }

            Neurons.ForEach((N, i) =>
            {
                N.InVal = values[i];
                N.OutVal = Activation.Activate(N.InVal);
            });
        }
    }
}