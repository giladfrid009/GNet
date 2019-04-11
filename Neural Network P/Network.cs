using PNet.Extensions;
using System;
using System.Collections.Generic;
using PNet.GlobalVars;

namespace PNet
{
    public class Network
    {        
        private readonly int[] structure;
        private readonly double[][] neurons;
        private readonly double[][] gradients;
        private readonly double[][] biases;
        private readonly double[][] biasesDelta;
        private readonly double[][][] weights;
        private readonly double[][][] weightsDelta;        

        public Network(int[] structure)
        {
            neurons = InitNeurons(structure);
            weights = InitWeights(structure);

            this.structure = structure.CopyByVal();

            gradients = neurons.CopyByVal();
            biases = neurons.CopyByVal();
            biasesDelta = biases.CopyByVal();
            weightsDelta = weights.CopyByVal();

            Randomize();
        }

        public Network(double[][] biases, double[][][] weights)
        {
            structure = CreateStructure(biases);

            this.biases = biases.CopyByVal();
            this.weights = weights.CopyByVal();

            neurons = biases.CopyByVal(0);
            gradients = neurons.CopyByVal();
            biasesDelta = neurons.CopyByVal();
            weightsDelta = weights.CopyByVal(0);
        }

        private int[] CreateStructure(double[][] biases)
        {
            int[] structure = new int[biases.Length];

            for (int i = 0; i < biases.Length; i++)
            {
                structure[i] = biases[i].Length;
            }

            return structure;
        }

        private double[][] InitNeurons(int[] structure)
        {
            double[][] neurons = new double[structure.Length][];

            for (int i = 0; i < structure.Length; i++)
            {
                neurons[i] = new double[structure[i]];
            }

            return neurons;
        }

        private double[][][] InitWeights(int[] structure)
        {
            double[][][] weights = new double[structure.Length][][];

            weights[0] = new double[0][];

            for (int i = 1; i < structure.Length; i++)
            {
                weights[i] = new double[structure[i]][];

                for (int j = 0; j < structure[i]; j++)
                {
                    weights[i][j] = new double[structure[i - 1]];
                }
            }

            return weights;
        }

        public void GetValues(out double[][] biases, out double[][][] weights)
        {
            biases = this.biases.CopyByVal();
            weights = this.weights.CopyByVal();
        }

        public void Randomize()
        {
            Random rnd = Globals.Rnd;

            for (int i = 0; i < structure.Length; i++)
            {
                for (int j = 0; j < structure[i]; j++)
                {
                    biases[i][j] = rnd.NextDouble(-1, 1);

                    if (i == 0)
                        continue;

                    for (int k = 0; k < structure[i - 1]; k++)
                    {
                        weights[i][j][k] = rnd.NextDouble(-1, 1);
                    }
                }
            }
        }

        public double[] Output(double[] inputs)
        {
            if (inputs.Length != structure[0])
                throw new ArgumentException("input length mismatch");

            for (int i = 0; i < structure[0]; i++)
            {
                neurons[0][i] = inputs[i];
            }

            for (int i = 1; i < structure.Length; i++)
            {
                for (int j = 0; j < structure[i]; j++)
                {
                    neurons[i][j] = 0;

                    for (int k = 0; k < structure[i - 1]; k++)
                    {
                        neurons[i][j] += neurons[i - 1][k] * weights[i][j][k];
                    }

                    neurons[i][j] = Sigmoid(neurons[i][j] + biases[i][j]);
                }
            }

            return neurons[neurons.Length - 1].CopyByVal();
        }

        public void Train(List<Data> trainingData, int maxEpochs = int.MaxValue, double minError = 0.0002, double learningRate = 0.4, double momentum = 0.9)
        {
            int outputIndex = structure.Length - 1;

            if (trainingData[0].Targets.Length != neurons[outputIndex].Length)
                throw new ArgumentException("training data targets length mismatch");

            for (int i = 0; i < maxEpochs; i++)
            {
                trainingData.Shuffle();

                double error = 0;

                for (int j = 0; j < trainingData.Count; j++)
                {
                    Output(trainingData[j].Inputs);

                    for (int k = 0; k < structure[outputIndex]; k++)
                    {
                        error += Math.Pow(trainingData[j].Targets[k] - neurons[outputIndex][k], 2);
                    }

                    Backpropogate(trainingData[j].Targets, learningRate, momentum);
                }

                error *= 0.5;

                if (error < minError)
                    break;
            }
        }

        private void Backpropogate(double[] targets, double learningRate, double momentum)
        {
            int outputIndex = structure.Length - 1;

            for (int i = 0; i < structure[outputIndex]; i++)
            {
                CalculateGradient(outputIndex, i, targets[i]);
                CalculateBias(outputIndex, i, learningRate, momentum);

                for (int k = 0; k < structure[outputIndex - 1]; k++)
                {
                    CalculateWeight(outputIndex, i, k, learningRate, momentum);
                }
            }

            for (int i = structure.Length - 2; i > 0; i--)
            {
                for (int j = 0; j < structure[i]; j++)
                {
                    CalculateGradient(i, j);
                    CalculateBias(i, j, learningRate, momentum);

                    for (int k = 0; k < structure[i - 1]; k++)
                    {
                        CalculateWeight(i, j, k, learningRate, momentum);
                    }
                }
            }
        }

        private void CalculateGradient(int neuronLayer, int neuronIndex)
        {
            gradients[neuronLayer][neuronIndex] = 0;

            for (int k = 0; k < structure[neuronLayer + 1]; k++)
            {
                gradients[neuronLayer][neuronIndex] += gradients[neuronLayer + 1][k] * weights[neuronLayer + 1][k][neuronIndex];
            }

            gradients[neuronLayer][neuronIndex] *= SigmoidDerivative(neurons[neuronLayer][neuronIndex]);
        }

        private void CalculateGradient(int neuronLayer, int neuronIndex, double targetValue)
        {
            gradients[neuronLayer][neuronIndex] = (targetValue - neurons[neuronLayer][neuronIndex]) * SigmoidDerivative(neurons[neuronLayer][neuronIndex]);
        }

        private void CalculateBias(int neuronLayer, int neuronIndex, double learningRate, double momentum)
        {
            double oldDelta = biasesDelta[neuronLayer][neuronIndex];
            biasesDelta[neuronLayer][neuronIndex] = learningRate * gradients[neuronLayer][neuronIndex];
            biases[neuronLayer][neuronIndex] += biasesDelta[neuronLayer][neuronIndex] + momentum * oldDelta;
        }

        private void CalculateWeight(int outNeuronLayer, int outNeuronIndex, int inNeuronIndex, double learningRate, double momentum)
        {
            double oldDelta = weightsDelta[outNeuronLayer][outNeuronIndex][inNeuronIndex];
            weightsDelta[outNeuronLayer][outNeuronIndex][inNeuronIndex] = learningRate * gradients[outNeuronLayer][outNeuronIndex] * neurons[outNeuronLayer - 1][inNeuronIndex];
            weights[outNeuronLayer][outNeuronIndex][inNeuronIndex] += weightsDelta[outNeuronLayer][outNeuronIndex][inNeuronIndex] + momentum * oldDelta;
        }

        private double Sigmoid(double value)
        {
            return 1 / (1 + Math.Exp(-value));
        }

        private double SigmoidDerivative(double neuronValue)
        {
            return neuronValue * (1 - neuronValue);
        }
    }
}
