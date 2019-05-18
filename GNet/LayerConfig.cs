using System;

namespace GNet
{
    public class LayerConfig : ICloneable
    {
        public int NeuronNum { get; private set; }
        public IActivation Activation { get; private set; }
        public IInitializer WeightsInit { get; private set; }
        public IInitializer BiasInit { get; private set; }

        public LayerConfig(int neuronNum, IActivation activation, IInitializer weightInit, IInitializer biasInit)
        {
            NeuronNum = neuronNum;
            Activation = activation;
            WeightsInit = weightInit;
            BiasInit = biasInit;
        }

        public object Clone()
        {
            return new LayerConfig(NeuronNum, (IActivation)Activation.Clone(), (IInitializer)WeightsInit.Clone(), (IInitializer)BiasInit.Clone());
        }
    }
}
