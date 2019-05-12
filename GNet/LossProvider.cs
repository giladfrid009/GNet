using System;
using static System.Math;

namespace GNet
{
    public delegate double LossFunc(double target, double output);

    public enum Losses { Squared, SquaredLog, Absolute, LogCosh, BinaryCrossEntropy, CategoricalCrossEntropy, Hinge, SquaredHinge, Poisson };

    public static class LossProvider
    {
        /// <summary>
        /// Returns a loss function.
        /// </summary>
        public static LossFunc GetLoss(Losses loss)
        {
            switch (loss)
            {
                case Losses.Squared: return (T, O) => (T - O) * (T - O);

                case Losses.SquaredLog: return (T, O) => Pow(Log(T + 1.0) - Log(O + 1.0), 2.0);

                case Losses.Absolute: return (T, O) => Abs(T - O);

                case Losses.LogCosh: return (T, O) => Log(Cosh(O - T));

                // till here all works.

                case Losses.BinaryCrossEntropy: return (T, O) => -T * Log(O) - (1.0 - T) * Log(1.0 - O);

                case Losses.CategoricalCrossEntropy: return (T, O) => -T * Log(O);

                case Losses.Hinge: return (T, O) => Max(0.0, 1.0 - T * O);

                case Losses.SquaredHinge: return (T, O) => Pow(Max(0.0, 1.0 - T * O), 2.0);

                case Losses.Poisson: return (T, O) => O - T * Log(O);

                default: throw new ArgumentOutOfRangeException("Unknown loss");
            }
        }

        /// <summary>
        /// Returns a loss derivative function.
        /// </summary>
        public static LossFunc GetDerivative(Losses loss)
        {
            switch (loss)
            {
                case Losses.Squared: return (T, O) => 2.0 * (O - T);

                case Losses.SquaredLog: return (T, O) => -2.0 * (Log(T + 1.0) - Log(O + 1.0)) / (O + 1.0);

                case Losses.Absolute: return (T, O) => T - O > 0.0 ? -1.0 : 1.0;

                case Losses.LogCosh: return (T, O) => Tanh(O - T);

                case Losses.BinaryCrossEntropy: return (T, O) => (T - O) / (O * O - O);

                case Losses.CategoricalCrossEntropy: return (T, O) => -T / O;

                case Losses.Hinge: return (T, O) => T * O < 1.0 ? -T : 0.0;

                case Losses.SquaredHinge: return (T, O) => T * O < 1.0 ? 2.0 * T * (T * O - 1.0) : 0.0;

                case Losses.Poisson: return (T, O) => 1.0 - (T / O);

                default: throw new ArgumentOutOfRangeException("Unknown loss");
            }
        }

        /// <summary>
        /// Calculates the avarage loss for the given outputs and targets.
        /// </summary>
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
