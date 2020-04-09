using System;
using GNet.Model;

namespace GNet.Layers
{
    [Serializable]
    public class Dense : TrainableLayer<Neuron>
    {
        public Dense(Shape shape, IActivation activation, IInitializer weightInit, IInitializer biasInit) : 
            base(shape, activation, biasInit, weightInit)
        {
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

        public override void Input(ShapedArrayImmutable<double> values)
        {
            if (values.Shape != Shape)
            {
                throw new ArgumentOutOfRangeException("Values shape mismatch.");
            }

            Neurons.ForEach((N, i) => N.Value = values[i]);

            ShapedArrayImmutable<double> activated = Activation.Activate(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }
    }
}