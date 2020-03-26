using static System.Math;

namespace GNet.Normalizers
{
    public class ZScore : INormalizer
    {        
        private double mean;
        private double sd;

        public void ExtractParams(ArrayImmutable<ShapedArrayImmutable<double>> dataVector)
        {
            mean = dataVector.Sum(D => D.Avarage()) / dataVector.Length;           
            
            double variance = dataVector.Sum(D => D.Sum(X => (X - mean) * (X - mean)));            

            int nVals = dataVector[0].Shape.Volume * dataVector.Length;

            sd = Sqrt(variance / nVals);
        }

        public ShapedArrayImmutable<double> Normalize(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => (X - mean) / sd);
        }

        public INormalizer Clone()
        {
            return new ZScore()
            {
                mean = mean,
                sd = sd
            };
        }
    }
}