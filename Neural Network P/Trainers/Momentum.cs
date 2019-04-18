using PNet.Extensions;

namespace PNet
{
    public class TrainerMomentum : TrainerBase
    {
        public double LearningRate;
        public double Momentum;

        protected readonly double[][] biasesDelta;
        protected readonly double[][][] weightsDelta;

        public TrainerMomentum(Network net, double learningRate = 0.4, double momentum = 0.9) : base(net)
        {
            LearningRate = learningRate;
            Momentum = momentum;

            biasesDelta = biases.CopyByVal(0);
            weightsDelta = weights.CopyByVal(0);
        }
        
        protected override void Backpropogate(double[] targets)
        {
            int outLayer = structure.Length - 1;

            for (int i = 0; i < structure[outLayer]; i++)
            {
                gradients[outLayer][i] = CalcGradient(outLayer, i, targets[i]);
                (biases[outLayer][i], biasesDelta[outLayer][i]) = CalcBias(outLayer, i);

                for (int j = 0; j < structure[outLayer - 1]; j++)
                {
                    (weights[outLayer][i][j], weightsDelta[outLayer][i][j]) = CalcWeight(outLayer, i, j);
                }
            }

            for (int i = structure.Length - 2; i > 0; i--)
            {
                for (int j = 0; j < structure[i]; j++)
                {
                    gradients[i][j] = CalcGradient(i, j);
                    (biases[i][j], biasesDelta[i][j]) = CalcBias(i, j);

                    for (int k = 0; k < structure[i - 1]; k++)
                    {
                        (weights[i][j][k], weightsDelta[i][j][k]) = CalcWeight(i, j, k);
                    }
                }
            }
        }

        private (double bias, double biasDelta) CalcBias(int biasLayer, int biasIndex)
        {
            double oldDelta = biasesDelta[biasLayer][biasIndex];
            double newDelta = CalcBiasDelta(biasLayer, biasIndex, LearningRate);
            double newBias = biases[biasLayer][biasIndex] + newDelta + Momentum * oldDelta;

            return (newBias, newDelta);
        }

        private (double weight, double weightDelta) CalcWeight(int outNeuronLayer, int outNeuronIndex, int inNeuronIndex)
        {
            double oldDelta = weightsDelta[outNeuronLayer][outNeuronIndex][inNeuronIndex];
            double newDelta = CalcWeightDelta(outNeuronLayer, outNeuronIndex, inNeuronIndex, LearningRate);
            double newWeight = weights[outNeuronLayer][outNeuronIndex][inNeuronIndex] + newDelta + Momentum * oldDelta;

            return (newWeight, newDelta);
        }
    }
}
