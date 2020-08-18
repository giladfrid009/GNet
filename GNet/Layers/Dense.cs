using GNet.Model;
using NCollections;
using System;

namespace GNet.Layers
{
    [Serializable]
    public class Dense : TrainableLayer
    {
        public override Array<Neuron> Neurons { get; }

        public Dense(Shape shape, IActivation activation, IInitializer? weightInit = null, IInitializer? biasInit = null) : base(shape, activation, weightInit, biasInit)
        {
            Neurons = new Array<Neuron>(shape.Volume, () => new Neuron());
        }

        public override void Connect(Layer inLayer)
        {
            Neurons.ForEach(outN => outN.InSynapses = inLayer.Neurons.Select(inN => new Synapse(inN, outN)));

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
    }
}