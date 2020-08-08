using static System.Math;

namespace GNet.Normalizers
{
    public class DecimalScale : INormalizer
    {
        public double Scale { get; private set; }

        public void UpdateParams(Dataset dataset, bool inputs, bool targets)
        {
            double maxI = 0.0;
            double maxT = 0.0;

            if (inputs)
            {
                maxI = dataset.DataCollection.Max(D => D.Inputs.Max(X => Abs(X)));
            }

            if (targets)
            {
                maxT = dataset.DataCollection.Max(D => D.Targets.Max(X => Abs(X)));
            }

            Scale = (int)Log10(Max(maxI, maxT)) + 1;
        }

        public double Normalize(double X)
        {
            return X / Scale;
        }
    }
}