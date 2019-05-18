using GNet.Extensions;

namespace GNet.Trainers
{
    public class TrainerMomentum : TrainerBase
    {
        public double LearningRate;
        public double Momentum;

        protected readonly double[][] biasesDelta;
        protected readonly double[][][] weightsDelta;

        public TrainerMomentum(Network network, ILoss loss, double learningRate = 0.4, double momentum = 0.9) : base(network, loss)
        {
            LearningRate = learningRate;
            Momentum = momentum;

            biasesDelta = batchBiases.DeepClone();
            weightsDelta = batchWeights.DeepClone();
            biasesDelta.ClearRecursive();
            weightsDelta.ClearRecursive();
        }

        protected override void BackPropogate(double[] targets)
        {
            int outLayer = layersConfig.Length - 1;

            // Output layer
            for (int j = 0; j < layersConfig[outLayer].NeuronNum; j++)
            {
                //var activationDerivative = ActivationProvider.GetDerivative(layersConfig[outLayer].Activation);
                var oldBiasDelta = biasesDelta[outLayer][j];

                //gradients[outLayer][j] = CalcGradient(outLayer, j, targets[j], activationDerivative);
                biasesDelta[outLayer][j] = CalcBiasDelta(outLayer, j, LearningRate);
                batchBiases[outLayer][j] += biasesDelta[outLayer][j] + oldBiasDelta * Momentum;

                for (int k = 0; k < layersConfig[outLayer - 1].NeuronNum; k++)
                {
                    var oldWeightDelta = weightsDelta[outLayer][j][k];

                    weightsDelta[outLayer][j][k] = CalcWeightDelta(outLayer, j, k, LearningRate);
                    batchWeights[outLayer][j][k] += weightsDelta[outLayer][j][k] + oldWeightDelta * Momentum;
                }
            }

            // Hidden & Input layers
            for (int i = outLayer - 1; i > 0; i--)
            {
                //var derivative = ActivationProvider.GetDerivative(layersConfig[i].Activation);

                for (int j = 0; j < layersConfig[i].NeuronNum; j++)
                {
                    var oldBiasDelta = biasesDelta[i][j];

                    //gradients[i][j] = CalcGradient(i, j, derivative);
                    biasesDelta[i][j] = CalcBiasDelta(i, j, LearningRate);
                    batchBiases[i][j] += biasesDelta[i][j] + oldBiasDelta * Momentum;

                    for (int k = 0; k < layersConfig[i - 1].NeuronNum; k++)
                    {
                        var oldWeightDelta = weightsDelta[i][j][k];

                        weightsDelta[i][j][k] = CalcWeightDelta(i, j, k, LearningRate);
                        batchWeights[i][j][k] += weightsDelta[i][j][k] + oldWeightDelta * Momentum;
                    }
                }
            }            
        }
    }
}
