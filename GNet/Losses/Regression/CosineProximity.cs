using static System.Math;

namespace GNet.Losses.Regression
{
    public class CosineProximity : ILoss
    {
        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            int i = 0;
            double dotProd = targets.Sum(T => T * outputs[i++]);

            double tNorm = Sqrt(targets.Sum(X => X * X));
            double oNorm = Sqrt(outputs.Sum(X => X * X));

            return dotProd / (tNorm * oNorm);
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            int i = 0;
            double dotProd = targets.Sum(T => T * outputs[i++]);

            double tNorm = Sqrt(targets.Sum(X => X * X));
            double oNorm = Sqrt(outputs.Sum(X => X * X));

            double A = 1.0 / (tNorm * oNorm);
            double B = dotProd / (oNorm * oNorm);

            return outputs.Combine(targets, (O, T) => A * (T - B * O));            
        }
    }
}