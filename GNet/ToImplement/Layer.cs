using System;
using System.Collections.Generic;
using System.Text;
using GNet.Extensions;

namespace GNet
{
    // todo: implement

    class Layer
    {        
        public Neuron[] Neurons { get; private set; }
        public IActivation Activation { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }

        public int Length
        {
            get { return Neurons.Length; }
        }

        public Layer(int neuronNum, IActivation activation, IInitializer weightInit, IInitializer biasInit)
        {          
            Activation = activation;
            WeightInit = weightInit;
            BiasInit = biasInit;
            Neurons = new Neuron[neuronNum];

            for (int i = 0; i < neuronNum; i++)
            {
                Neurons[i] = new Neuron();
            }
        }

        public object Clone()
        {            
            return new LayerConfig(Length, (IActivation)Activation.Clone(), (IInitializer)WeightInit.Clone(), (IInitializer)BiasInit.Clone());
        }
    }
}
