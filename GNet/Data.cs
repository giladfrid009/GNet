namespace GNet
{
    public class Data : ICloneable<Data>
    {
        public ShapedReadOnlyArray<double> Inputs { get; }
        public ShapedReadOnlyArray<double> Outputs { get; }

        public Data(ShapedReadOnlyArray<double> inputs, ShapedReadOnlyArray<double> outputs)
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
