namespace GNet.Trainers
{
    public class TrainerClassic : TrainerBase
    {
        public double LearningRate;

        public TrainerClassic(Network network, ILoss loss, double learningRate = 0.4) : base(network, loss)
        {
            LearningRate = learningRate;
        }

        protected override void BackPropogate(double[] targets)
        {
            int outLayer = layersConfig.Length - 1;

            // Output layer

            var lossDer = loss.Derivative(targets, activNeurons[outLayer]);
            var activDer = layersConfig[outLayer].Activation.Derivative(neurons[outLayer]);

            for (int j = 0; j < layersConfig[outLayer].NeuronNum; j++)
            {
                gradients[outLayer][j] = CalcGradient(lossDer[j], activDer[j]);
                batchBiases[outLayer][j] += CalcBiasDelta(outLayer, j, LearningRate);

                for (int k = 0; k < layersConfig[outLayer - 1].NeuronNum; k++)
                {
                    batchWeights[outLayer][j][k] += CalcWeightDelta(outLayer, j, k, LearningRate);
                }
            }

            // Hidden & Input layers
            for (int i = outLayer - 1; i > 0; i--)
            {
                activDer = layersConfig[i].Activation.Derivative(neurons[i]);

                for (int j = 0; j < layersConfig[i].NeuronNum; j++)
                {
                    gradients[i][j] = CalcGradient(j, activDer[i], gradients[i + 1], weights[i + 1]);
                    batchBiases[i][j] += CalcBiasDelta(i, j, LearningRate);

                    for (int k = 0; k < layersConfig[i - 1].NeuronNum; k++)
                    {
                        batchWeights[i][j][k] += CalcWeightDelta(i, j, k, LearningRate);
                    }
                }
            }            
        }
    }
}
