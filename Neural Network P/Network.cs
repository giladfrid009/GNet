using PNet.Extensions;
using PNet.GlobalVars;
using System;

namespace PNet
{
    public class Network
    {
        private readonly int[] structure;
        private readonly double[][] neurons;
        private readonly double[][] biases;
        private readonly double[][][] weights;

        public Network(int[] structure)
        {
            neurons = InitNeurons(structure);
            weights = InitWeights(structure);
            this.structure = structure.CopyByVal();
            biases = neurons.CopyByVal();
        }

        public Network(double[][] biases, double[][][] weights)
        {
            structure = CreateStructure(biases);
            this.biases = biases.CopyByVal();
            this.weights = weights.CopyByVal();
            neurons = biases.CopyByVal(0);
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

        public (int[] structure, double[][] neurons, double[][] biases, double[][][] weights) GetParamRefs()
        {
            return (structure, neurons, biases, weights);
        }

        public (int[] structure, double[][] neurons, double[][] biases, double[][][] weights) GetParamCopies()
        {
            return (structure.CopyByVal(), neurons.CopyByVal(), biases.CopyByVal(), weights.CopyByVal());
        }

        public void InitializeRandomly(double minWeight = -1, double maxWeight = 1)
        {
            Random rnd = Globals.Rnd;

            for (int i = 0; i < structure.Length; i++)
            {
                for (int j = 0; j < structure[i]; j++)
                {
                    biases[i][j] = 0;

                    if (i == 0)
                        continue;

                    for (int k = 0; k < structure[i - 1]; k++)
                    {
                        weights[i][j][k] = rnd.NextDouble(minWeight, maxWeight);
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

        private double Sigmoid(double value)
        {
            return 1 / (1 + Math.Exp(-value));
        }
    }
}
