using GNet.Extensions;
using System;
using System.Collections.Generic;

namespace GNet.Trainers
{
    public abstract class TrainerBase
    {
        private readonly Network network;
        private readonly LossFunc lossFunc;
        private readonly LossFunc lossDerivative;
        private readonly double[][] biases;
        private readonly double[][] neurons;
        private readonly double[][][] weights;

        protected readonly LayerConfig[] layersConfig;
        protected readonly double[][] batchBiases;
        protected readonly double[][] gradients;
        protected readonly double[][][] batchWeights;

        public TrainerBase(Network refNetwork, Losses loss)
        {
            (layersConfig, neurons, biases, weights) = refNetwork.GetParamRefs();

            network = refNetwork;

            lossFunc = LossProvider.GetLoss(loss);
            lossDerivative = LossProvider.GetDerivative(loss);

            gradients = neurons.DeepClone();
            batchBiases = biases.DeepClone();
            batchWeights = weights.DeepClone();
            batchBiases.ClearRecursive();
            batchWeights.ClearRecursive();
        }

        private void TestDataStructure(List<Data> data)
        {
            int outIndex = layersConfig.Length - 1;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Inputs.Length != layersConfig[0].NeuronNum)
                {
                    throw new Exception($"Data[{i}].Inputs length mismatch with network structure");
                }
                else if (data[i].Targets.Length != layersConfig[outIndex].NeuronNum)
                {
                    throw new Exception($"Data[{i}].Targets length mismatch with network structure");
                }
            }
        }

        public void Train(List<Data> trainingData, int batchSize, int maxEpochs, double minAvgError)
        {
            TestDataStructure(trainingData);

            for (int i = 0; i < maxEpochs; i++)
            {
                trainingData.Shuffle();

                double epochError = 0;
                int maxIndex = 0;
                int index = 0;

                for (; maxIndex <= trainingData.Count; maxIndex += batchSize)
                {
                    if (maxIndex > trainingData.Count)
                        maxIndex = trainingData.Count;

                    for (; index < maxIndex; index++)
                    {
                        var outputs = network.Output(trainingData[index].Inputs);

                        epochError += LossProvider.CalcTotalLoss(lossFunc, trainingData[index].Targets, outputs);

                        // todo: calcDeltas should probably return (biasesDelta, weightsDelta). find best structure
                        calcDeltas(trainingData[index].Targets);
                    }

                    UpdateNetwork();

                    batchBiases.ClearRecursive();
                    batchWeights.ClearRecursive();
                }

                epochError /= trainingData.Count;

                if (epochError < minAvgError)
                    break;
            }
        }

        protected abstract void calcDeltas(double[] targets);

        protected virtual double CalcGradient(int neuronLayer, int neuronIndex, ActivationFunc activationDerivative)
        {
            double gradient = 0;

            for (int k = 0; k < layersConfig[neuronLayer + 1].NeuronNum; k++)
            {
                gradient += gradients[neuronLayer + 1][k] * weights[neuronLayer][neuronIndex][k];
            }

            return gradient *= activationDerivative(neurons[neuronLayer][neuronIndex]);
        }

        protected virtual double CalcGradient(int neuronLayer, int neuronIndex, double targetValue, ActivationFunc activationDerivative)
        {
            return -1 * lossDerivative(targetValue, neurons[neuronLayer][neuronIndex]) * activationDerivative(neurons[neuronLayer][neuronIndex]);
        }

        protected virtual double CalcBiasDelta(int biasLayer, int biasIndex, double learningRate)
        {
            return learningRate * gradients[biasLayer][biasIndex];
        }

        protected virtual double CalcWeightDelta(int inNeuronLayer, int inNeuronIndex, int outNeuronIndex, double learningRate)
        {
            return learningRate * gradients[inNeuronLayer + 1][outNeuronIndex] * neurons[inNeuronLayer][inNeuronIndex];
        }

        private void UpdateNetwork()
        {
            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] += batchWeights[i][j][k];
                    }
                }
            }

            for (int i = 0; i < biases.Length; i++)
            {
                for (int j = 0; j < biases[i].Length; j++)
                {
                    biases[i][j] += batchBiases[i][j];
                }
            }
        }
    }
}
