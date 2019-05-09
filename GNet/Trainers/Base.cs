using GNet.Extensions;
using System;
using System.Collections.Generic;

namespace GNet.Trainers
{
    public abstract class TrainerBase
    {
        protected readonly Network network;
        protected readonly LayerConfig[] layersConfig;
        protected readonly Losses lossFunc;
        protected readonly double[][] biases;
        protected readonly double[][] batchBiases;
        protected readonly double[][] neurons;
        protected readonly double[][] gradients;
        protected readonly double[][][] weights;
        protected readonly double[][][] batchWeights;

        public TrainerBase(Network refNetwork, Losses lossFunc)
        {
            (layersConfig, neurons, biases, weights) = refNetwork.GetParamRefs();

            network = refNetwork;
            this.lossFunc = lossFunc;

            gradients = neurons.DeepClone();
            batchBiases = biases.DeepClone(0);
            batchWeights = weights.DeepClone(0);
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

        // todo: implement batch
        // todo: implement 2 training methods: based on minError and based on maxEpoches, or keep them null as default, and check for them only if they have val
        public void Train(List<Data> trainingData, int batchSize, int maxEpochs = int.MaxValue, double minAvgError = 0.002)
        {
            TestDataStructure(trainingData);

            for (int i = 0; i < maxEpochs; i++)
            {
                trainingData.Shuffle();

                double epochError = 0;

                for (int j = 0; j < trainingData.Count; j++)
                {
                    var outputs = network.Output(trainingData[j].Inputs);

                    epochError += LossProvider.GetTotalLoss(lossFunc, trainingData[j].Targets, outputs);                    

                    Backpropogate(trainingData[j].Targets);
                }

                epochError /= trainingData.Count;

                if (epochError < minAvgError)
                    break;
            }
        }        

        protected abstract void Backpropogate(double[] targets);

        protected virtual double CalcGradient(int neuronLayer, int neuronIndex, ActivationFunc activationDerivative)
        {
            double gradient = 0;

            for (int k = 0; k < layersConfig[neuronLayer + 1].NeuronNum; k++)
            {
                gradient += gradients[neuronLayer + 1][k] * weights[neuronLayer][neuronIndex][k];
            }

            return gradient *= activationDerivative(neurons[neuronLayer][neuronIndex]);
        }

        protected virtual double CalcGradient(int neuronLayer, int neuronIndex, double targetValue, ActivationFunc activationDerivative, LossFunc lossDerivative)
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
    }
}
