using static System.Math;

namespace GNet.Normalizers
{
    public class MinMax : INormalizer
    {
        public double MinVal { get; private set; }
        public double MaxVal { get; private set; }

        public void UpdateParams(Dataset dataset, bool inputs, bool targets)
        {
            double minI = 0.0;
            double maxI = 0.0;
            double minT = 0.0;
            double maxT = 0.0;

            if (inputs)
            {
                minI = dataset.DataCollection.Min(D => D.Inputs.Min());
                maxI = dataset.DataCollection.Max(D => D.Inputs.Max());
            }

            if (targets)
            {
                minT = dataset.DataCollection.Min(D => D.Targets.Min());
                maxT = dataset.DataCollection.Max(D => D.Targets.Max());
            }

            MinVal = Min(minI, minT);
            MaxVal = Max(maxI, maxT);
        }

        public double Normalize(double X)
        {
            return MinVal == MaxVal ? 0.0 : (X - MinVal) / (MaxVal - MinVal);
        }
    }
}