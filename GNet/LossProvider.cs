using System;
using static System.Math;

namespace GNet
{
    public enum LossFuncs { Squared, SquaredLog, Absolute, AbsolutePrecent, Hinge, HingeSquared, RelativeEntropy, CrossEntropy, NegativeLogLiklihood, Poisson };

    public static class LossProvider
    {

        // todo: comparing to https://github.com/dotnet/machinelearning/blob/master/src/Microsoft.ML.Data/Utils/LossFunctions.cs, a lot of losses and their derivatives are wrong. wtf? why and understand logic.

        public static double GetLoss(LossFuncs lossFunc, double T, double O)
        {
            switch(lossFunc)
            {
                case LossFuncs.Squared: return (T - O) * (T - O);

                case LossFuncs.SquaredLog: return Log(T + 1) - Log(O + 1);

                case LossFuncs.Absolute: return Abs(T - O);

                case LossFuncs.AbsolutePrecent: return Abs((T - O) / T) * 100;

                case LossFuncs.Hinge: return Max(0, 1 - T * O);

                case LossFuncs.HingeSquared: return Pow(Max(0, 1 - T * O), 2);

                case LossFuncs.RelativeEntropy: return T * Log(T / O);

                case LossFuncs.CrossEntropy: return -1 * (T * Log(O) + (1 - T) * Log(1 - O));

                case LossFuncs.NegativeLogLiklihood: return -Log(O);

                case LossFuncs.Poisson: return O - T * Log(O);

                default: throw new ArgumentOutOfRangeException("Unknown lossFunc");
            }
        }         
        
        public static double Derive(LossFuncs lossFunc, double T, double O)
        {
            switch (lossFunc)
            {
                case LossFuncs.Squared: return 2 * (O - T);

                case LossFuncs.SquaredLog: return -1 / (O + 1);

                case LossFuncs.Absolute: return (O - T) / Abs(T - O);

                case LossFuncs.AbsolutePrecent: return 100 * O * (T - O) / (Pow(T, 3) * Abs(1 - O / T));

                case LossFuncs.Hinge: return T * O < 1 ? -T : 0;

                case LossFuncs.HingeSquared: return T * O < 1 ? 2 * T * (T * O - 1) : 0;

                case LossFuncs.RelativeEntropy: return -T / O;

                case LossFuncs.CrossEntropy: return (T - O) / (O * (O - 1));

                case LossFuncs.NegativeLogLiklihood: return -1 / O;

                case LossFuncs.Poisson: return 1 - (T / O);

                default: throw new ArgumentOutOfRangeException("Unknown lossFunc");
            }
        }

        // todo: does it belong here? or move it out?
        public static double GetTotalLoss(LossFuncs lossFunc, double[] targets, double[] outputs)
        {
            if (targets.Length != outputs.Length)
                throw new ArgumentException("targets and outputs length mismatch");

            double totalLoss = 0;

            for (int i = 0; i < targets.Length; i++)
            {
                totalLoss += GetLoss(lossFunc, targets[i], outputs[i]);
            }

            return totalLoss / targets.Length;
        }
    }
}
