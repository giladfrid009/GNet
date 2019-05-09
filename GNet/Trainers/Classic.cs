namespace GNet.Trainers
{
    public class TrainerClassic : TrainerBase
    {
        public double LearningRate;

        public TrainerClassic(Network net, Losses lossFunc, double learningRate = 0.4) : base(net, lossFunc)
        {
            LearningRate = learningRate;
        }      

        protected override void Backpropogate(double[] targets)
        {
            int outLayer = layersConfig.Length - 1;

            // Output layer
            for (int j = 0; j < layersConfig[outLayer].NeuronNum; j++)
            {
                var activationDerivative = ActivationProvider.GetDerivative(layersConfig[outLayer].Activation);
                
                // todo: lossDerivative may be a global network func.
                var lossDerivative = LossProvider.GetDerivative(lossFunc);

                gradients[outLayer][j] = CalcGradient(outLayer, j, targets[j], activationDerivative, lossDerivative);
                biases[outLayer][j] += CalcBiasDelta(outLayer, j, LearningRate);
            }

            // Hidden layers
            for (int i = outLayer - 1; i > 0; i--)
            {
                var derivative = ActivationProvider.GetDerivative(layersConfig[i].Activation);

                for (int j = 0; j < layersConfig[i].NeuronNum; j++)
                {
                    gradients[i][j] = CalcGradient(i, j, derivative);
                    biases[i][j] += CalcBiasDelta(i, j, LearningRate);

                    for (int k = 0; k < layersConfig[i + 1].NeuronNum; k++)
                    {
                        weights[i][j][k] += CalcWeightDelta(i, j, k, LearningRate);
                    }
                }
            }

            // Input layer
            for (int j = 0; j < layersConfig[0].NeuronNum; j++)
            {
                for (int k = 0; k < layersConfig[j + 1].NeuronNum; k++)
                {
                    weights[0][j][k] += CalcWeightDelta(0, j, k, LearningRate);
                }
            }
        }
    }
}
