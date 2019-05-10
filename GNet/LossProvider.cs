using System;
using static System.Math;

namespace GNet
{
    public delegate double LossFunc(double target, double output);

    public enum Losses { Squared, SquaredLog, Absolute, AbsolutePrecent, Hinge, HingeSquared, RelativeEntropy, CrossEntropy, NegativeLogLiklihood, Poisson };

    public static class LossProvider
    {
        // todo: comparing to https://github.com/dotnet/machinelearning/blob/master/src/Microsoft.ML.Data/Utils/LossFunctions.cs, a lot of losses and their derivatives are wrong. wtf? why and understand logic.

        public static LossFunc GetLoss(Losses loss)
        {
            switch (loss)
            {
                case Losses.Squared: return (T, O) => (T - O) * (T - O);

                case Losses.SquaredLog: return (T, O) => Log(T + 1.0) - Log(O + 1.0);

                case Losses.Absolute: return (T, O) => Abs(T - O);

                case Losses.AbsolutePrecent: return (T, O) => Abs((T - O) / T) * 100.0;

                case Losses.Hinge: return (T, O) => Max(0.0, 1.0 - T * O);

                case Losses.HingeSquared: return (T, O) => Pow(Max(0.0, 1.0 - T * O), 2.0);

                case Losses.RelativeEntropy: return (T, O) => T * Log(T / O);

                case Losses.CrossEntropy: return (T, O) => -1.0 * (T * Log(O) + (1.0 - T) * Log(1.0 - O));

                case Losses.NegativeLogLiklihood: return (T, O) => -Log(O);

                case Losses.Poisson: return (T, O) => O - T * Log(O);

                default: throw new ArgumentOutOfRangeException("Unknown loss");
            }
        }

        public static LossFunc GetDerivative(Losses loss)
        {
            switch (loss)
            {
                case Losses.Squared: return (T, O) => 2.0 * (O - T);

                case Losses.SquaredLog: return (T, O) => -1.0 / (O + 1);

                case Losses.Absolute: return (T, O) => (O - T) / Abs(T - O);

                case Losses.AbsolutePrecent: return (T, O) => 100.0 * O * (T - O) / (Pow(T, 3.0) * Abs(1.0 - O / T));

                case Losses.Hinge: return (T, O) => T * O < 1.0 ? -T : 0.0;

                case Losses.HingeSquared: return (T, O) => T * O < 1.0 ? 2.0 * T * (T * O - 1.0) : 0.0;

                case Losses.RelativeEntropy: return (T, O) => -T / O;

                case Losses.CrossEntropy: return (T, O) => (T - O) / (O * (O - 1.0));

                case Losses.NegativeLogLiklihood: return (T, O) => -1.0 / O;

                case Losses.Poisson: return (T, O) => 1.0 - (T / O);

                default: throw new ArgumentOutOfRangeException("Unknown loss");
            }
        }

        public static double CalcTotalLoss(LossFunc lossFunc, double[] targets, double[] outputs)
        {
            if (targets.Length != outputs.Length)
                throw new ArgumentException("targets and outputs length mismatch");

            double totalLoss = 0;

            for (int i = 0; i < targets.Length; i++)
            {
                totalLoss += lossFunc(targets[i], outputs[i]);
            }

            return totalLoss / targets.Length;
        }
    }
}
