using static System.Math;

namespace GNet.Normalizers
{
    public class ZScore : INormalizer
    {
        public double Avg { get; private set; }
        public double SD { get; private set; }

        public void UpdateParams(Dataset dataset, bool inputs, bool targets)
        {
            int nElems = 0;
            double sumI = 0.0;
            double sumT = 0.0;

            if(inputs)
            {
                sumI = dataset.Sum(D => D.Inputs.Sum());

                nElems += dataset.InputShape.Volume * dataset.Length;
            }

            if (targets)
            {
                sumT = dataset.Sum(D => D.Targets.Sum());

                nElems += dataset.TargetShape.Volume * dataset.Length;
            }

            Avg = (sumI + sumT) / nElems;

            double var = 0.0;

            if (inputs)
            {
                var += dataset.Sum(D => D.Inputs.Sum(X => (X - Avg) * (X - Avg)));
            }

            if (targets)
            {
                var += dataset.Sum(D => D.Targets.Sum(X => (X - Avg) * (X - Avg)));
            }

            SD = Sqrt((var + double.Epsilon) / nElems);
        }

        public double Normalize(double X)
        {
            return (X - Avg) / SD;
        }
    }
}