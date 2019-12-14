namespace GNet
{
    public class Data : ICloneable<Data>
    {
        public ShapedArray<double> Inputs { get; }
        public ShapedArray<double> Outputs { get; }

        public Data(ShapedArray<double> inputs, ShapedArray<double> outputs)
        {
            Inputs = inputs.Clone();
            Outputs = outputs.Clone();
        }

        public Data Clone()
        {
            return new Data(Inputs, Outputs);
        }
    }
}
