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
            neurons = CreateNeuronsArr(layersConfig);
            biases = CreateBiasArr(layersConfig);
            weights = CreateWeightsArr(layersConfig);

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

        public (LayerConfig[] LayersConfig, double[][] Neurons, double[][] Biases, double[][][] Weights) GetParamRefs()
        {
            return (layersConfig, neurons, biases, weights);
        }

        public (LayerConfig[] LayersConfig, double[][] Neurons, double[][] Biases, double[][][] Weights) GetParamCopies()
        {
            return (layersConfig.DeepClone(), neurons.DeepClone(), biases.DeepClone(), weights.DeepClone());
        }

        private double[][] CreateNeuronsArr(LayerConfig[] layersConfig)
        {
            double[][] neurons = new double[layersConfig.Length][];

            for (int i = 0; i < layersConfig.Length; i++)
            {
                neurons[i] = new double[layersConfig[i].NeuronNum];
            }

            return neurons;
        }

        private double[][] CreateBiasArr(LayerConfig[] layersConfig)
        {
            double[][] biases = new double[layersConfig.Length][];
            biases[0] = new double[0];

            for (int i = 1; i < layersConfig.Length; i++)
            {
                biases[i] = new double[layersConfig[i].NeuronNum];
            }

            return biases;
        }

        private double[][][] CreateWeightsArr(LayerConfig[] layersConfig)
        {
            double[][][] weights = new double[layersConfig.Length][][];

            weights[0] = new double[0][];

            for (int i = 1; i < layersConfig.Length; i++)
            {
                weights[i] = new double[layersConfig[i].NeuronNum][];

                for (int j = 0; j < layersConfig[i].NeuronNum; j++)
                {
                    weights[i][j] = new double[layersConfig[i - 1].NeuronNum];
                }
            }

            return weights;
        }

        public void InitNetwork()
        {
            for (int i = 1; i < weights.Length; i++)
            {
                var nIn = layersConfig[i - 1].NeuronNum;
                var nOut = layersConfig[i].NeuronNum;

                for (int j = 0; j < weights[i].Length; j++)
                {
                    var initializer = InitializerProvider.GetInitializer(layersConfig[i].BiasInitializer);

                    biases[i][j] = initializer(nIn, nOut);

                    initializer = InitializerProvider.GetInitializer(layersConfig[i].WeightsInitializer);

                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = initializer(nIn, nOut);
                    }
                }
            }            
        }

        public double Validate(List<Data> testData, Losses loss)
        {
            double lossAvg = 0;
            LossFunc lossFunc = LossProvider.GetLoss(loss);
           
            for (int i = 0; i < testData.Count; i++)
            {
                lossAvg += LossProvider.CalcTotalLoss(lossFunc, testData[i].Targets, Output(testData[i].Inputs));
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
                        neurovVal += neurons[i - 1][k] * weights[i][j][k];
                    }

                    neurons[i][j] = activationFunc(neurovVal + biases[i][j]);
                }
            }

            return neurons[neurons.Length - 1].DeepClone();
        }
    }
}
