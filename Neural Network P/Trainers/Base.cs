using System;
using System.Linq;
using System.Collections.Generic;
using PNet.Extensions;

namespace PNet
{
    public abstract class TrainerBase
    {
        protected readonly Network network;
        protected readonly int[] structure;
        protected readonly double[][] biases;
        protected readonly double[][] neurons;
        protected readonly double[][] gradients;
        protected readonly double[][][] weights;

        public TrainerBase (Network refNetwork)
        {
            (structure, neurons, biases, weights) = refNetwork.GetParamRefs();

            network = refNetwork;
            gradients = neurons.CopyByVal();
        }

        private void TestDataStructure(List<Data> data)
        {
            int outIndex = structure.Length - 1;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Inputs.Length != structure[0])
                {
                    throw new Exception($"Data[{i}].Inputs length mismatch with network structure");
                }
                else if (data[i].Targets.Length != structure[outIndex])
                {
                    throw new Exception($"Data[{i}].Targets length mismatch with network structure");
                }
            }
        }       

        // todo: implement batch
        public void Train(List<Data> trainingData, int batchSize, int maxEpochs = int.MaxValue, double minError = 0.002)
        {
            TestDataStructure(trainingData);

            int outIndex = structure.Length - 1;

            for (int i = 0; i < maxEpochs; i++)
            {
                trainingData.Shuffle();

                double epochError = 0;

                for (int j = 0; j < trainingData.Count; j++)
                {
                    network.Output(trainingData[j].Inputs);

                    for (int k = 0; k < structure[outIndex]; k++)
                    {
                        var localError = trainingData[j].Targets[k] - neurons[outIndex][k];
                        epochError += localError * localError;
                    }

                    Backpropogate(trainingData[j].Targets);
                }

                if (epochError < minError)
                    break;
            }
        }

        public double Validate(List<Data> testData, double[] ErrorMargins)
        {
            TestDataStructure(testData);

            int incorrect = 0;
            int outIndex = structure.Length - 1;
            int outLength = structure[structure.Length - 1];

            if (ErrorMargins.Length != outLength)
                throw new Exception("acceptableErrMargin length mismatch with ooutput length.");

            for (int i = 0; i < testData.Count; i++)
            {
                network.Output(testData[i].Inputs);

                for (int j = 0; j < outLength; j++)
                {
                    var error = Math.Abs(neurons[outIndex][j] - testData[i].Targets[j]);

                    if (error > ErrorMargins[j])
                    {
                        incorrect++;
                        break;
                    }
                }
            }

            return (double)(testData.Count - incorrect) / testData.Count;
        }

        protected abstract void Backpropogate(double[] targets);        

        protected double CalcGradient(int neuronLayer, int neuronIndex)
        {
            double gradient = 0;

            for (int k = 0; k < structure[neuronLayer + 1]; k++)
            {
                gradient += gradients[neuronLayer + 1][k] * weights[neuronLayer + 1][k][neuronIndex];
            }

            return gradient *= SigmoidDerivative(neurons[neuronLayer][neuronIndex]);
        }

        protected double CalcGradient(int neuronLayer, int neuronIndex, double targetValue)
        {
            return (targetValue - neurons[neuronLayer][neuronIndex]) * SigmoidDerivative(neurons[neuronLayer][neuronIndex]);
        }

        protected double CalcBiasDelta(int biasLayer, int biasIndex, double learningRate)
        {
            return learningRate * gradients[biasLayer][biasIndex];
        }

        protected double CalcWeightDelta(int outNeuronLayer, int outNeuronIndex, int inNeuronIndex, double learningRate)
        {
            return learningRate * gradients[outNeuronLayer][outNeuronIndex] * neurons[outNeuronLayer - 1][inNeuronIndex];
        }

        private double SigmoidDerivative(double neuronValue)
        {
            return neuronValue * (1 - neuronValue);
        }
    }
}
