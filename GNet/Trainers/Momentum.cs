using GNet.Extensions;

namespace GNet.Trainers
{
    public class TrainerMomentum : TrainerBase
    {
        public double LearningRate;
        public double Momentum;

        protected readonly double[][] biasesDelta;
        protected readonly double[][][] weightsDelta;

        public TrainerMomentum(Network net, Losses lossFunc, double learningRate = 0.4, double momentum = 0.9) : base(net, lossFunc)
        {
            LearningRate = learningRate;
            Momentum = momentum;

            biasesDelta = biases.DeepClone(0);
            weightsDelta = weights.DeepClone(0);
        }
        
        protected override void Backpropogate(double[] targets)
        {
            int outLayer = layersConfig.Length - 1;

            // Output layer
            for (int j = 0; j < layersConfig[outLayer].NeuronNum; j++)
            {
                var activationDerivative = ActivationProvider.GetDerivative(layersConfig[outLayer].Activation);

                var lossDerivative = LossProvider.GetDerivative(lossFunc);

                gradients[outLayer][j] = CalcGradient(outLayer, j, targets[j], activationDerivative, lossDerivative);
                (biases[outLayer][j], biasesDelta[outLayer][j]) = CalcBias(outLayer, j, LearningRate, Momentum);
            }

            // Hidden layers
            for (int i = outLayer - 1; i > 0; i--)
            {
                var derivative = ActivationProvider.GetDerivative(layersConfig[i].Activation);

                for (int j = 0; j < layersConfig[i].NeuronNum; j++)
                {
                    gradients[i][j] = CalcGradient(i, j, derivative);
                    (biases[i][j], biasesDelta[i][j]) = CalcBias(i, j, LearningRate, Momentum);

                    for (int k = 0; k < layersConfig[i + 1].NeuronNum; k++)
                    {
                        (weights[i][j][k], weightsDelta[i][j][k]) = CalcWeight(i, j, k, LearningRate, Momentum);
                    }
                }
            }

            // Input layer
            for (int j = 0; j < layersConfig[0].NeuronNum; j++)
            {
                for (int k = 0; k < layersConfig[j + 1].NeuronNum; k++)
                {
                    (weights[0][j][k], weightsDelta[0][j][k]) = CalcWeight(0, j, k, LearningRate, Momentum);
                }
            }
        }

        protected virtual (double bias, double biasDelta) CalcBias(int biasLayer, int biasIndex, double LearningRate, double Momentum)
        {
            double oldDelta = biasesDelta[biasLayer][biasIndex];
            double newDelta = CalcBiasDelta(biasLayer, biasIndex, LearningRate);
            double newBias = biases[biasLayer][biasIndex] + newDelta + Momentum * oldDelta;

            return (newBias, newDelta);
        }

       protected virtual (double weight, double weightDelta) CalcWeight(int inNeuronLayer, int inNeuronIndex, int outNeuronIndex, double LearningRate, double Momentum)
        {
            double oldDelta = weightsDelta[inNeuronLayer][inNeuronIndex][outNeuronIndex];
            double newDelta = CalcWeightDelta(inNeuronLayer, inNeuronIndex, outNeuronIndex, LearningRate);
            double newWeight = weights[inNeuronLayer][inNeuronIndex][outNeuronIndex] + newDelta + Momentum * oldDelta;

            return (newWeight, newDelta);
        }
    }
}
