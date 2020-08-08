using static System.Math;

namespace GNet.Normalizers
{
    public class ZScore : INormalizer
    {
        public double Mean { get; private set; }
        public double SD { get; private set; }

        public void UpdateParams(Dataset dataset, bool inputs, bool targets)
        {
            int nElems = 0;
            double sumI = 0.0;
            double sumT = 0.0;

            if (inputs)
            {
                sumI = dataset.DataCollection.Sum(D => D.Inputs.Sum());
                nElems += dataset.InputShape.Volume * dataset.Length;
            }

            if (targets)
            {
                sumT = dataset.DataCollection.Sum(D => D.Targets.Sum());
                nElems += dataset.TargetShape.Volume * dataset.Length;
            }

            Mean = (sumI + sumT) / nElems;

            double var = 0.0;

            if (inputs)
            {
                var += dataset.DataCollection.Sum(D => D.Inputs.Sum(X => (X - Mean) * (X - Mean)));
            }

            if (targets)
            {
                var += dataset.DataCollection.Sum(D => D.Targets.Sum(X => (X - Mean) * (X - Mean)));
            }

            SD = Sqrt(var / nElems);
        }

        public double Normalize(double X)
        {
            return (X - Mean) / SD;
        }
    }
}