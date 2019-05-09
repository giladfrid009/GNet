using System;
using static System.Math;

namespace GNet
{
    public delegate double LossFunc(double target, double output);

    public enum Losses { Squared, SquaredLog, Absolute, AbsolutePrecent, Hinge, HingeSquared, RelativeEntropy, CrossEntropy, NegativeLogLiklihood, Poisson };

    public static class LossProvider
    {
        // todo: comparing to https://github.com/dotnet/machinelearning/blob/master/src/Microsoft.ML.Data/Utils/LossFunctions.cs, a lot of losses and their derivatives are wrong. wtf? why and understand logic.

        public static LossFunc GetLoss(Losses lossFunc)
        {
            switch(lossFunc)
            {
                case Losses.Squared: return (T, O) => (T - O) * (T - O);

                case Losses.SquaredLog: return (T, O) => Log(T + 1) - Log(O + 1);

                case Losses.Absolute: return (T, O) => Abs(T - O);

                case Losses.AbsolutePrecent: return (T, O) => Abs((T - O) / T) * 100;

                case Losses.Hinge: return (T, O) => Max(0, 1 - T * O);

                case Losses.HingeSquared: return (T, O) => Pow(Max(0, 1 - T * O), 2);

                case Losses.RelativeEntropy: return (T, O) => T * Log(T / O);

                case Losses.CrossEntropy: return (T, O) => -1 * (T * Log(O) + (1 - T) * Log(1 - O));

                case Losses.NegativeLogLiklihood: return (T, O) => -Log(O);

                case Losses.Poisson: return (T, O) => O - T * Log(O);

                default: throw new ArgumentOutOfRangeException("Unknown lossFunc");
            }
        }         
        
        public static LossFunc GetDerivative(Losses lossFunc)
        {
            switch (lossFunc)
            {
                case Losses.Squared: return (T, O) => 2 * (O - T);

                case Losses.SquaredLog: return (T, O) => -1 / (O + 1);

                case Losses.Absolute: return (T, O) => (O - T) / Abs(T - O);

                case Losses.AbsolutePrecent: return (T, O) => 100 * O * (T - O) / (Pow(T, 3) * Abs(1 - O / T));

                case Losses.Hinge: return (T, O) => T * O < 1 ? -T : 0;

                case Losses.HingeSquared: return (T, O) => T * O < 1 ? 2 * T * (T * O - 1) : 0;

                case Losses.RelativeEntropy: return (T, O) => -T / O;

                case Losses.CrossEntropy: return (T, O) => (T - O) / (O * (O - 1));

                case Losses.NegativeLogLiklihood: return (T, O) => -1 / O;

                case Losses.Poisson: return (T, O) => 1 - (T / O);

                default: throw new ArgumentOutOfRangeException("Unknown lossFunc");
            }
        }

        // todo: does it belong here? or move it out?
        public static double GetTotalLoss(Losses lossFunc, double[] targets, double[] outputs)
        {
            if (targets.Length != outputs.Length)
                throw new ArgumentException("targets and outputs length mismatch");

            double totalLoss = 0;

            var loss = GetLoss(lossFunc);

            for (int i = 0; i < targets.Length; i++)
            {
                totalLoss += loss(targets[i], outputs[i]);
            }

            return totalLoss / targets.Length;
        }
    }
}
