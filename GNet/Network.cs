using GNet.Extensions;
using System;
using System.Collections.Generic;

namespace GNet
{
    public class Network
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
            weights = InitWeightsArr(layersData);
            biases = neurons.DeepClone();

            InitNetwork();
        }

        public Network(LayerData[] layersData, double[][] biases, double[][][] weights)
        {
            this.layersData = layersData.DeepClone();
            this.biases = biases.DeepClone();
            this.weights = weights.DeepClone();
            neurons = biases.DeepClone(0);
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

        // todo: imlement instead of copying neuron array.
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
            biases = biases.DeepClone(0);

            for (int i = 0; i < layersData.Length - 1; i++)
            {
                WeightProvider.InitializeLayer(weights, i, layersData[i].LayerWeightInit);
            }
        }

        public double Validate(List<Data> testData, double[] ErrorMargins)
        {
            if (ErrorMargins.Length != layersData[layersData.Length - 1].NeuronNum)
                throw new Exception("ErrorMargin length mismatch with output length.");

            int correctNum = 0;
            int outLength = layersData[layersData.Length - 1].NeuronNum;

            for (int i = 0; i < testData.Count; i++)
            {
                bool isCorrect = true;
                double[] output =  Output(testData[i].Inputs);

                for (int j = 0; j < outLength; j++)
                {
                    if (Math.Abs(output[j] - testData[i].Targets[j]) > ErrorMargins[j] || output[j] == double.NaN)
                    {
                        isCorrect = false;
                        break;
                    }
                }

                if (isCorrect)
                    correctNum++;
            }

            return correctNum / (double)testData.Count;
        }

        public double[] Output(double[] inputs)
        {
            if (inputs.Length != layersData[0].NeuronNum)
                throw new ArgumentException("input length mismatch");

            for (int i = 0; i < layersData[0].NeuronNum; i++)
            {
                neurons[0][i] = Activations.ActivateValue(inputs[i], layersData[0].LayerActivation);
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

                    neurons[i][j] = Activations.ActivateValue(neurovVal + biases[i][j], layersData[i].LayerActivation);
                }
            }

            return neurons[neurons.Length - 1].DeepClone();
        }       
    }
}
