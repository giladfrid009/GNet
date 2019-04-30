using GNet.Extensions;
using System;
using System.Collections.Generic;

namespace GNet
{
    public class Network : ICloneable
    {
        private static Random rnd = new Random();

        private LayerData[] layersData;
        private double[][] neurons;
        private double[][] biases;
        private double[][][] weights;

        public Network(LayerData[] layersData)
        {
            this.layersData = layersData.DeepClone();
            neurons = InitNeuronsArr(layersData);
            biases = InitBiasArr(layersData);
            weights = InitWeightsArr(layersData);

            InitNetwork();
        }

        public Network(LayerData[] layersData, double[][] biases, double[][][] weights)
        {
            this.layersData = layersData.DeepClone();
            this.biases = biases.DeepClone();
            this.weights = weights.DeepClone();
            neurons = biases.DeepClone(0);
        }

        public object Clone()
        {
            return new Network(layersData, biases, weights);
        }

        public (LayerData[] layersData, double[][] neurons, double[][] biases, double[][][] weights) GetParamRefs()
        {
            return (layersData, neurons, biases, weights);
        }

        public (LayerData[] layersData, double[][] neurons, double[][] biases, double[][][] weights) GetParamCopies()
        {
            return (layersData.DeepClone(), (double[][])neurons.DeepClone(), biases.DeepClone(), weights.DeepClone());
        }

        private double[][] InitNeuronsArr(LayerData[] layersData)
        {
            double[][] neurons = new double[layersData.Length][];

            for (int i = 0; i < layersData.Length; i++)
            {
                neurons[i] = new double[layersData[i].NeuronNum];
            }

            return neurons;
        }

        private double[][] InitBiasArr(LayerData[] layersData)
        {
            double[][] biases = new double[layersData.Length][];
            biases[0] = new double[0];

            for (int i = 1; i < layersData.Length; i++)
            {
                biases[i] = new double[layersData[i].NeuronNum];
            }

            return biases;
        }

        private double[][][] InitWeightsArr(LayerData[] layersData)
        {
            double[][][] weights = new double[layersData.Length][][];

            for (int i = 0; i < layersData.Length - 1; i++)
            {
                weights[i] = new double[layersData[i].NeuronNum][];

                for (int j = 0; j < layersData[i].NeuronNum; j++)
                {
                    weights[i][j] = new double[layersData[i + 1].NeuronNum];
                }
            }

            weights[weights.Length - 1] = new double[0][];

            return weights;
        }     
        
        public void InitNetwork()
        {
            for (int i = 0; i < weights.Length; i++)
            {
                var initializer = layersData[i].WeightsInitializer;
                var prevLength = weights[i].Length; // todo: weights reside on the wrong layer: weights of layer index 1 are on index 0.

                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = Initializer.GenerateValue(initializer, prevLength);
                    }
                }
            }

            for (int i = 1; i < biases.Length; i++)
            {
                var initializer = layersData[i].BiasInitializer;
                var prevLength = biases[i - 1].Length;

                for (int j = 0; j < biases[i].Length; j++)
                {
                    biases[i][j] = Initializer.GenerateValue(initializer, prevLength);
                }
            }
        }

        public double Validate(List<Data> testData, LossFuncs lossFunc)
        {
            double lossAvg = 0;

            for (int i = 0; i < testData.Count; i++)
            {
                lossAvg += LossProvider.GetTotalLoss(lossFunc, testData[i].Targets, Output(testData[i].Inputs));
            }

            return lossAvg / testData.Count;
        }

        public double[] Output(double[] inputs)
        {
            if (inputs.Length != layersData[0].NeuronNum)
                throw new ArgumentException("input length mismatch");

            for (int i = 0; i < layersData[0].NeuronNum; i++)
            {
                neurons[0][i] = ActivationProvider.ActivateValue(layersData[0].ActivationFunction, inputs[i]);
            }

            for (int i = 1; i < layersData.Length; i++)
            {
                for (int j = 0; j < layersData[i].NeuronNum; j++)
                {
                    double neurovVal = 0;

                    for (int k = 0; k < layersData[i - 1].NeuronNum; k++)
                    {
                        neurovVal += neurons[i - 1][k] * weights[i - 1][k][j];
                    }

                    neurons[i][j] = ActivationProvider.ActivateValue(layersData[i].ActivationFunction, neurovVal + biases[i][j]);
                }
            }

            return neurons[neurons.Length - 1].DeepClone();
        }       
    }
}
