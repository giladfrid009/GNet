namespace GNet.Trainers
{
    public class TrainerClassic : TrainerBase
    {
        public double LearningRate;

        public TrainerClassic(Network net, Losses loss, double learningRate = 0.4) : base(net, loss)
        {
            LearningRate = learningRate;
        }

        protected override void calcDeltas(double[] targets)
        {
            int outLayer = layersConfig.Length - 1;

            // Output layer
            for (int j = 0; j < layersConfig[outLayer].NeuronNum; j++)
            {
                var activationDerivative = ActivationProvider.GetDerivative(layersConfig[outLayer].Activation);

                gradients[outLayer][j] = CalcGradient(outLayer, j, targets[j], activationDerivative, lossDerivative);
                batchBiases[outLayer][j] += CalcBiasDelta(outLayer, j, LearningRate);
            }

            // Hidden layers
            for (int i = outLayer - 1; i > 0; i--)
            {
                var derivative = ActivationProvider.GetDerivative(layersConfig[i].Activation);

                for (int j = 0; j < layersConfig[i].NeuronNum; j++)
                {
                    gradients[i][j] = CalcGradient(i, j, derivative);
                    batchBiases[i][j] += CalcBiasDelta(i, j, LearningRate);

                    for (int k = 0; k < layersConfig[i + 1].NeuronNum; k++)
                    {
                        batchWeights[i][j][k] += CalcWeightDelta(i, j, k, LearningRate);
                    }
                }
            }

            // Input layer
            for (int j = 0; j < layersConfig[0].NeuronNum; j++)
            {
                for (int k = 0; k < layersConfig[j + 1].NeuronNum; k++)
                {
                    batchWeights[0][j][k] += CalcWeightDelta(0, j, k, LearningRate);
                }
            }
        }
    }
}
