namespace PNet
{
    public class TrainerClassic : TrainerBase
    {
        public double LearningRate;

        public TrainerClassic(Network net, double learningRate = 0.4) : base(net)
        {
            LearningRate = learningRate;
        }      

        protected override void Backpropogate(double[] targets)
        {
            int outLayer = structure.Length - 1;

            for (int i = 0; i < structure[outLayer]; i++)
            {
                gradients[outLayer][i] = CalcGradient(outLayer, i, targets[i]);
                biases[outLayer][i] += CalcBiasDelta(outLayer, i, LearningRate);

                for (int j = 0; j < structure[outLayer - 1]; j++)
                {
                    weights[outLayer][i][j] += CalcWeightDelta(outLayer, i, j, LearningRate);
                }
            }

            for (int i = structure.Length - 2; i > 0; i--)
            {
                for (int j = 0; j < structure[i]; j++)
                {
                    gradients[i][j] = CalcGradient(i, j);
                    biases[i][j] += CalcBiasDelta(i, j, LearningRate);

                    for (int k = 0; k < structure[i - 1]; k++)
                    {
                        weights[i][j][k] += CalcWeightDelta(i, j, k, LearningRate);
                    }
                }
            }
        }
    }
}
