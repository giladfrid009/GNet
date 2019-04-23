using System;

namespace GNet
{
    // todo: implement connection type
    public enum ConnectionTypes { Dense };

    public class LayerData : ICloneable
    {
        public int NeuronNum { get; private set; }
        public ActivationFunctions LayerActivation { get; private set; }
        public WeightInitializers LayerWeightInit { get; private set; }
        public ConnectionTypes LayerConnection { get; private set; }

        public LayerData(int neuronNum, ConnectionTypes connectionType, ActivationFunctions activationFunction, WeightInitializers weightInitializer)
        {
            NeuronNum = neuronNum;
            LayerConnection = connectionType;
            LayerActivation = activationFunction;
            LayerWeightInit = weightInitializer;
        }

        public object Clone()
        {
            return new LayerData(NeuronNum, LayerConnection, LayerActivation, LayerWeightInit);
        }        
    }
}
