using System;

namespace GNet
{
    // todo: implement connection type
    public enum ConnectionTypes { Dense };

    public class LayerData : ICloneable
    {
        public int NeuronNum { get; private set; }
        public Activations ActivationFunction { get; private set; }
        public Initializers WeightsInitializer { get; private set; }
        public Initializers BiasInitializer { get; private set; }
        public ConnectionTypes LayerConnection { get; private set; }

        public LayerData(int neuronNum, ConnectionTypes connectionType, Activations activationFunction, Initializers weightInitializer, Initializers biasInitializer)
        {
            NeuronNum = neuronNum;
            LayerConnection = connectionType;
            ActivationFunction = activationFunction;
            WeightsInitializer = weightInitializer;
            BiasInitializer = biasInitializer;
        }

        public object Clone()
        {
            return new LayerData(NeuronNum, LayerConnection, ActivationFunction, WeightsInitializer, BiasInitializer);
        }        
    }
}
