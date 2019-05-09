using System;

namespace GNet
{
    // todo: implement connection type
    public enum ConnectionTypes { Dense };

    public class LayerConfig : ICloneable
    {
        public int NeuronNum { get; private set; }
        public Activations Activation { get; private set; }
        public Initializers WeightsInitializer { get; private set; }
        public Initializers BiasInitializer { get; private set; }
        public ConnectionTypes ConnectionType { get; private set; }

        public LayerConfig(int neuronNum, ConnectionTypes connectionType, Activations activationFunction, Initializers weightInitializer, Initializers biasInitializer)
        {
            NeuronNum = neuronNum;
            ConnectionType = connectionType;
            Activation = activationFunction;
            WeightsInitializer = weightInitializer;
            BiasInitializer = biasInitializer;
        }

        public object Clone()
        {
            return new LayerConfig(NeuronNum, ConnectionType, Activation, WeightsInitializer, BiasInitializer);
        }        
    }
}
