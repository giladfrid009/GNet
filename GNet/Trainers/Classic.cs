namespace GNet.Trainers
{
    public class TrainerClassic : TrainerBase
    {
        public double LearningRate;

        public TrainerClassic(Network net, LossFuncs lossFunc, double learningRate = 0.4) : base(net, lossFunc)
        {
            LearningRate = learningRate;
        }      

        protected override void Backpropogate(double[] targets)
        {
            int outLayer = layersData.Length - 1;

            // Output layer
            for (int j = 0; j < layersData[outLayer].NeuronNum; j++)
            {
                gradients[outLayer][j] = CalcGradient(outLayer, j, targets[j]);
                biases[outLayer][j] += CalcBiasDelta(outLayer, j, LearningRate);
            }

            // Hidden layers
            for (int i = outLayer - 1; i > 0; i--)
            {
                for (int j = 0; j < layersData[i].NeuronNum; j++)
                {
                    gradients[i][j] = CalcGradient(i, j);

                    for (int k = 0; k < layersData[i + 1].NeuronNum; k++)
                    {
                        weights[i][j][k] += CalcWeightDelta(i, j, k, LearningRate);
                    }
                }
            }

            // Input layer
            for (int j = 0; j < layersData[0].NeuronNum; j++)
            {
                for (int k = 0; k < layersData[j + 1].NeuronNum; k++)
                {
                    weights[0][j][k] += CalcWeightDelta(0, j, k, LearningRate);
                }
            }
        }
    }
}
