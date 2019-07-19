using GNet.Extensions.Generic;

namespace GNet
{
    public class Data : ICloneable<Data>
    {
        public double[] Inputs { get; private set; }
        public double[] Outputs { get; private set; }

        public Data(double[] inputs, double[] outputs)
        {
            Inputs = inputs.Select(X => X);
            Outputs = outputs.Select(X => X);
        } 
        
        public void Normalize(INormalizer inputNormalizer, INormalizer outputNormalizer)
        {
            Inputs = inputNormalizer.Normalize(Inputs);
            Outputs = outputNormalizer.Normalize(Outputs);
        }

        public Data Clone() => new Data(Inputs, Outputs);
    }
}
