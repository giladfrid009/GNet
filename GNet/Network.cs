using GNet.Extensions;
using System;
using System.Collections.Generic;

namespace GNet
{
    public class Network : ICloneable
    {
        private readonly LayerConfig[] layersConfig;
        private readonly double[][] neurons;
        private readonly double[][] biases;
        private readonly double[][][] weights;

        public Network(LayerConfig[] layersConfig)
        {
            this.layersConfig = layersConfig.DeepClone();
            neurons = createNeuronsArr(layersConfig);
            biases = createBiasArr(layersConfig);
            weights = createWeightsArr(layersConfig);

            InitNetwork();
        }

        public Network(LayerConfig[] layersConfig, double[][] biases, double[][][] weights)
        {
            this.layersConfig = layersConfig.DeepClone();
            this.biases = biases.DeepClone();
            this.weights = weights.DeepClone();
            neurons = biases.DeepClone();
            neurons.ClearRecursive();
        }

        public object Clone()
        {
            return new Network(layersConfig, biases, weights);
        }

        public (LayerConfig[] layersConfig, double[][] neurons, double[][] biases, double[][][] weights) GetParamRefs()
        {
            return (layersConfig, neurons, biases, weights);
        }

        public (LayerConfig[] layersConfig, double[][] neurons, double[][] biases, double[][][] weights) GetParamCopies()
        {
            return (layersConfig.DeepClone(), neurons.DeepClone(), biases.DeepClone(), weights.DeepClone());
        }

        private double[][] createNeuronsArr(LayerConfig[] layersConfig)
        {
            double[][] neurons = new double[layersConfig.Length][];

            for (int i = 0; i < layersConfig.Length; i++)
            {
                neurons[i] = new double[layersConfig[i].NeuronNum];
            }

            return neurons;
        }

        private double[][] createBiasArr(LayerConfig[] layersConfig)
        {
            double[][] biases = new double[layersConfig.Length][];
            biases[0] = new double[0];

            for (int i = 1; i < layersConfig.Length; i++)
            {
                biases[i] = new double[layersConfig[i].NeuronNum];
            }

            return biases;
        }

        private double[][][] createWeightsArr(LayerConfig[] layersConfig)
        {
            double[][][] weights = new double[layersConfig.Length][][];

            for (int i = 0; i < layersConfig.Length - 1; i++)
            {
                weights[i] = new double[layersConfig[i].NeuronNum][];

                for (int j = 0; j < layersConfig[i].NeuronNum; j++)
                {
                    weights[i][j] = new double[layersConfig[i + 1].NeuronNum];
                }
            }

            weights[weights.Length - 1] = new double[0][];

            return weights;
        }

        public void InitNetwork()
        {
            for (int i = 0; i < weights.Length - 1; i++)
            {
                // todo: weights reside on the wrong layer: weights of layer index 1 are on index 0.
                // weights should be moved 1 index further
                var initializer = InitializerProvider.GetInitializer(layersConfig[i].WeightsInitializer);
                var nIn = weights[i].Length;
                var nOut = weights[i + 1].Length;

                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = initializer(nIn, nOut);
                    }
                }
            }

            for (int i = 1; i < biases.Length; i++)
            {
                var initializer = InitializerProvider.GetInitializer(layersConfig[i].BiasInitializer);
                var nIn = biases[i - 1].Length;
                var nOut = biases[i].Length;

                for (int j = 0; j < nOut; j++)
                {
                    biases[i][j] = initializer(nIn, nOut);
                }
            }
        }

        public double Validate(List<Data> testData, Losses loss)
        {
            double lossAvg = 0;
           
            for (int i = 0; i < testData.Count; i++)
            {
                lossAvg += LossProvider.CalcTotalLoss(LossProvider.GetLoss(loss), testData[i].Targets, Output(testData[i].Inputs));
            }

            lossAvg /= testData.Count;

            return lossAvg;
        }

        public double[] Output(double[] inputs)
        {
            if (inputs.Length != layersConfig[0].NeuronNum)
                throw new ArgumentException("input length mismatch");

            var activationFunc = ActivationProvider.GetActivation(layersConfig[0].Activation);

            for (int i = 0; i < layersConfig[0].NeuronNum; i++)
            {
                neurons[0][i] = activationFunc(inputs[i]);
            }

            for (int i = 1; i < layersConfig.Length; i++)
            {
                activationFunc = ActivationProvider.GetActivation(layersConfig[i].Activation);

                for (int j = 0; j < layersConfig[i].NeuronNum; j++)
                {
                    double neurovVal = 0;

                    for (int k = 0; k < layersConfig[i - 1].NeuronNum; k++)
                    {
                        neurovVal += neurons[i - 1][k] * weights[i - 1][k][j];
                    }

                    neurons[i][j] = activationFunc(neurovVal + biases[i][j]);
                }
            }

            return neurons[neurons.Length - 1].DeepClone();
        }
    }
}
