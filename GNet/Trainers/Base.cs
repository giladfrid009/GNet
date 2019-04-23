using GNet.Extensions;
using System;
using System.Collections.Generic;

namespace GNet.Trainers
{
    public abstract class TrainerBase
    {
        // todo: maybe remove bias from here?

        protected readonly Network network;
        protected readonly LayerData[] layersData;
        protected readonly double[][] biases;
        protected readonly double[][] neurons;
        protected readonly double[][] gradients;
        protected readonly double[][][] weights;

        public TrainerBase(Network refNetwork)
        {
            (layersData, neurons, biases, weights) = refNetwork.GetParamRefs();

            network = refNetwork;
            gradients = neurons.DeepClone();
        }

        private void TestDataStructure(List<Data> data)
        {
            int outIndex = layersData.Length - 1;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Inputs.Length != layersData[0].NeuronNum)
                {
                    throw new Exception($"Data[{i}].Inputs length mismatch with network structure");
                }
                else if (data[i].Targets.Length != layersData[outIndex].NeuronNum)
                {
                    throw new Exception($"Data[{i}].Targets length mismatch with network structure");
                }
            }
        }

        // todo: implement batch
        public void Train(List<Data> trainingData, int batchSize, int maxEpochs = int.MaxValue, double minError = 0.02)
        {
            TestDataStructure(trainingData);

            int outIndex = layersData.Length - 1;

            for (int i = 0; i < maxEpochs; i++)
            {
                trainingData.Shuffle();

                double epochError = 0;

                for (int j = 0; j < trainingData.Count; j++)
                {
                    network.Output(trainingData[j].Inputs);

                    for (int k = 0; k < layersData[outIndex].NeuronNum; k++)
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

        protected abstract void Backpropogate(double[] targets);

        protected virtual double CalcGradient(int neuronLayer, int neuronIndex)
        {
            double gradient = 0;

            for (int k = 0; k < layersData[neuronLayer + 1].NeuronNum; k++)
            {
                gradient += gradients[neuronLayer + 1][k] * weights[neuronLayer][neuronIndex][k];
            }
           
            return gradient *= Activations.Derivative(neurons[neuronLayer][neuronIndex], layersData[neuronLayer].LayerActivation);
        }

        protected virtual double CalcGradient(int neuronLayer, int neuronIndex, double targetValue)
        {
            return CalcError(targetValue, neurons[neuronLayer][neuronIndex]) * Activations.Derivative(neurons[neuronLayer][neuronIndex], layersData[neuronLayer].LayerActivation);
        }

        protected virtual double CalcBiasDelta(int biasLayer, int biasIndex, double learningRate)
        {
            return learningRate * gradients[biasLayer][biasIndex];
        }       

        protected virtual double CalcWeightDelta(int inNeuronLayer, int inNeuronIndex, int outNeuronIndex, double learningRate)
        {
            return learningRate * gradients[inNeuronLayer + 1][outNeuronIndex] * neurons[inNeuronLayer][inNeuronIndex];
        }

        protected virtual double CalcError(double targetValue, double value)
        {
            return targetValue - value;
        }
    }
}
