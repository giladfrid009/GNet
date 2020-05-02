using static System.Math;

namespace GNet.Normalizers
{
    public class MinMax : INormalizer
    {
        public double Min { get; private set; }
        public double Max { get; private set; }

        public void UpdateParams(Dataset dataset, bool inputs, bool targets)
        {
            double minI = 0.0;
            double maxI = 0.0;
            double minT = 0.0;
            double maxT = 0.0;

            if (inputs)
            {
                minI = dataset.Min(D => D.Inputs.Min());
                maxI = dataset.Max(D => D.Inputs.Max());
            }

            if (targets)
            {
                minT = dataset.Min(D => D.Targets.Min());
                maxT = dataset.Max(D => D.Targets.Max());
            }

            Min = Min(minI, minT);
            Max = Max(maxI, maxT);

            if (Min == Max)
            {
                Max += double.Epsilon;
            }
        }

        public double Normalize(double X)
        {
            return (X - Min) / (Max - Min);
        }
    }
}