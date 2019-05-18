using GNet.Extensions;
using System;

namespace GNet
{
    public class Network : ICloneable
    {
        // todo: rename layersConfig for clarity, or change class name of LayerConfig
        private readonly LayerConfig[] layersConfig;
        private readonly double[][] neurons;
        private readonly double[][] aNeurons;
        private readonly double[][] biases;
        private readonly double[][][] weights;

        public Network(LayerConfig[] layersConfig)
        {
            this.layersConfig = layersConfig.DeepClone();
            neurons = CreateNeuronsArr(layersConfig);
            biases = CreateBiasArr(layersConfig);
            weights = CreateWeightsArr(layersConfig);
            aNeurons = neurons.DeepClone();

            InitNetwork();
        }

        public Network(LayerConfig[] layersConfig, double[][] biases, double[][][] weights)
        {
            this.layersConfig = layersConfig.DeepClone();
            this.biases = biases.DeepClone();
            this.weights = weights.DeepClone();
            neurons = biases.DeepClone();
            neurons.ClearRecursive();
            aNeurons = neurons.DeepClone();
        }

        public object Clone()
        {
            return new Network(layersConfig, biases, weights);
        }

        public (LayerConfig[] LayersConfig, double[][] Neurons, double[][] ActivatedNeurons, double[][] Biases, double[][][] Weights) GetParamRefs()
        {
            return (layersConfig, neurons, aNeurons, biases, weights);
        }

        public (LayerConfig[] LayersConfig, double[][] Biases, double[][][] Weights) GetParamClones()
        {
            return (layersConfig.DeepClone(), biases.DeepClone(), weights.DeepClone());
        }

        private double[][] CreateNeuronsArr(LayerConfig[] layersConfig)
        {
            return layersConfig.Map(C => new double[C.NeuronNum]);
        }

        private double[][] CreateBiasArr(LayerConfig[] layersConfig)
        {
            double[][] biases = layersConfig.Map(C => new double[C.NeuronNum]);
            biases[0] = new double[0];

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
                    biases[i][j] = layersConfig[i].BiasInit.Init(nIn, nOut);

                    var initializer = layersConfig[i].WeightsInit;

                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = initializer.Init(nIn, nOut);
                    }
                }
            }            
        }

        public double Validate(Data[] testData, ILoss loss)
        {
            return testData.Map(X => loss.Compute(X.Targets, Output(X.Inputs))).Accumulate(0.0, (R, X) => R + X);
        }

        public double[] Output(double[] inputs)
        {
            if (inputs.Length != layersConfig[0].NeuronNum)
                throw new ArgumentException("input length mismatch");

            neurons[0] = layersConfig[0].Activation.Activate(inputs);

            for (int i = 1; i < layersConfig.Length; i++)
            {
                for (int j = 0; j < layersConfig[i].NeuronNum; j++)
                {
                    double neurovVal = 0;

                    for (int k = 0; k < layersConfig[i - 1].NeuronNum; k++)
                    {
                        neurovVal += neurons[i - 1][k] * weights[i][j][k];
                    }

                    neurons[i][j] = neurovVal + biases[i][j];
                }

                aNeurons[i] = layersConfig[i].Activation.Activate(neurons[i]);
            }

            return aNeurons[neurons.Length - 1].DeepClone();
        }
    }
}
