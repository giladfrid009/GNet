using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using static System.Math;

namespace GNet.Normalizers
{
    // todo: shoulf calc mean and sd of whole dataset.
    public class ZScore : INormalizer
    {
        public enum DataVector { Input, Output }

        public double Mean { get; }
        public double SD { get; }

        public ZScore(Dataset dataset, DataVector dataVector)
        {
            if (dataVector == DataVector.Input)
            {
                Mean = dataset.DataCollection.Sum(D => D.Inputs.Avarage()) / dataset.DataLength;
                SD = Sqrt(dataset.DataCollection.Sum(D => D.Inputs.Sum(X => (X - Mean) * (X - Mean))) / (dataset.DataLength * dataset.InputLength));
            }
            else
            {
                Mean = dataset.DataCollection.Sum(D => D.Outputs.Avarage()) / dataset.DataLength;
                SD = Sqrt(dataset.DataCollection.Sum(D => D.Outputs.Sum(X => (X - Mean) * (X - Mean))) / (dataset.DataLength * dataset.OutputLength));
            }         
        }

        private ZScore(double mean, double sd)
        {
            Mean = mean;
            SD = sd;
        }

        public double[] Normalize(double[] vals)
        {
            return vals.Select(X => (X - Mean) / SD);
        }

        public INormalizer Clone()
        {
            return new ZScore(Mean, SD);
        }
    }
}
