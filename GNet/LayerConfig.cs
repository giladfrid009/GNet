using System;

namespace GNet
{
    public class LayerConfig : ICloneable
    {
        public int NeuronNum { get; private set; }
        public Activations Activation { get; private set; }
        public Initializers WeightsInitializer { get; private set; }
        public Initializers BiasInitializer { get; private set; }

        public LayerConfig(int neuronNum, Activations activationFunction, Initializers weightInitializer, Initializers biasInitializer)
        {
            NeuronNum = neuronNum;
            Activation = activationFunction;
            WeightsInitializer = weightInitializer;
            BiasInitializer = biasInitializer;
        }

        public object Clone()
        {
            return new LayerConfig(NeuronNum, Activation, WeightsInitializer, BiasInitializer);
        }
    }
}
