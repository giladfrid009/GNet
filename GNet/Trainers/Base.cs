using GNet.Extensions;
using System;
using System.Diagnostics;

namespace GNet.Trainers
{
    public abstract class TrainerBase
    {
        private readonly Network network;
        private readonly double[][] biases;
        protected readonly double[][] neurons;
        protected readonly double[][] activNeurons;
        protected readonly double[][][] weights;

        protected readonly ILoss loss;
        protected readonly LayerConfig[] layersConfig;
        protected readonly double[][] batchBiases;
        protected readonly double[][] gradients;
        protected readonly double[][][] batchWeights;

        public TrainerBase(Network network, ILoss loss)
        {
            this.network = network;
            this.loss = loss;

            (layersConfig, neurons, activNeurons, biases, weights) = this.network.GetParamRefs();

            gradients = neurons.DeepClone();
            batchBiases = biases.DeepClone();
            batchWeights = weights.DeepClone();
        }

        private void TestDataStructure(Data[] data)
        {
            int outIndex = layersConfig.Length - 1;

            for (int i = 0; i < data.Length; i++)
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

        public void Train(Data[] trainingData, int batchSize, bool shuffle = true, int maxEpochs = int.MaxValue, double minError = double.NaN)
        {
            TestDataStructure(trainingData);

            for (int i = 0; i < maxEpochs; i++)
            {
                var epochData = shuffle ? trainingData.Shuffle() : trainingData;

                var epochError = 0.0;
                var maxIndex = 0;
                var index = 0;

                for (; maxIndex <= epochData.Length; maxIndex += batchSize)
                {
                    if (maxIndex > epochData.Length)
                        maxIndex = epochData.Length;

                    batchBiases.ClearRecursive();
                    batchWeights.ClearRecursive();

                    for (; index < maxIndex; index++)
                    {
                        if (minError != double.NaN)
                            epochError += loss.Compute(epochData[index].Targets, network.Output(epochData[index].Inputs));

                        // todo: calcDeltas should probably return (biasesDelta, weightsDelta). find best structure
                        BackPropogate(epochData[index].Targets);
                    }

                    UpdateNetwork();
                }

                if (minError != double.NaN)
                {
                    epochError /= epochData.Length;

                    if (epochError < minError)
                        break;
                }                
            }
        }

        protected abstract void BackPropogate(double[] targets);

        protected double CalcGradient(int neuronIndex ,double activDer, double[] outGrads, double[][] outWeights)
        {
            double grad = 0;

            for (int k = 0; k < outGrads.Length; k++)
            {
                grad += outGrads[k] * outWeights[k][neuronIndex];
            }

            return grad *= activDer;
        }

        protected double CalcGradient(double lossDer, double activDer)
        {
            return -1 * lossDer * activDer;
        }        

        protected double CalcBiasDelta(int biasLayer, int biasIndex, double learningRate)
        {
            return learningRate * gradients[biasLayer][biasIndex];
        }

        protected double CalcWeightDelta(int outNeuronLayer, int outNeuronIndex, int inNeuronIndex, double learningRate)
        {
            return learningRate * gradients[outNeuronLayer][outNeuronIndex] * neurons[outNeuronLayer - 1][inNeuronIndex];
        }

        private void UpdateNetwork()
        {
            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    biases[i][j] += batchBiases[i][j];

                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] += batchWeights[i][j][k];
                    }
                }
            }            
        }
    }
}
