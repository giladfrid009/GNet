namespace GNet.Datasets
{
    public static class LogicGates
    {
        public static Dataset OR { get; } = new Dataset
        (
            new Data(new Tensor<double>(0.0, 0.0), new Tensor<double>(0.0)),
            new Data(new Tensor<double>(0.0, 1.0), new Tensor<double>(1.0)),
            new Data(new Tensor<double>(1.0, 0.0), new Tensor<double>(1.0)),
            new Data(new Tensor<double>(1.0, 1.0), new Tensor<double>(1.0))
        );

        public static Dataset AND { get; } = new Dataset
        (
            new Data(new Tensor<double>(0.0, 0.0), new Tensor<double>(0.0)),
            new Data(new Tensor<double>(0.0, 1.0), new Tensor<double>(0.0)),
            new Data(new Tensor<double>(1.0, 0.0), new Tensor<double>(0.0)),
            new Data(new Tensor<double>(1.0, 1.0), new Tensor<double>(1.0))
        );

        public static Dataset XOR { get; } = new Dataset
        (
            new Data(new Tensor<double>(0.0, 0.0), new Tensor<double>(0.0)),
            new Data(new Tensor<double>(0.0, 1.0), new Tensor<double>(1.0)),
            new Data(new Tensor<double>(1.0, 0.0), new Tensor<double>(1.0)),
            new Data(new Tensor<double>(1.0, 1.0), new Tensor<double>(0.0))
        );

        public static Dataset NOR { get; } = new Dataset
        (
            new Data(new Tensor<double>(0.0, 0.0), new Tensor<double>(1.0)),
            new Data(new Tensor<double>(0.0, 1.0), new Tensor<double>(0.0)),
            new Data(new Tensor<double>(1.0, 0.0), new Tensor<double>(0.0)),
            new Data(new Tensor<double>(1.0, 1.0), new Tensor<double>(0.0))
        );

        public static Dataset NAND { get; } = new Dataset
        (
            new Data(new Tensor<double>(0.0, 0.0), new Tensor<double>(1.0)),
            new Data(new Tensor<double>(0.0, 1.0), new Tensor<double>(1.0)),
            new Data(new Tensor<double>(1.0, 0.0), new Tensor<double>(1.0)),
            new Data(new Tensor<double>(1.0, 1.0), new Tensor<double>(0.0))
        );

        public static Dataset XNOR { get; } = new Dataset
        (
            new Data(new Tensor<double>(0.0, 0.0), new Tensor<double>(1.0)),
            new Data(new Tensor<double>(0.0, 1.0), new Tensor<double>(0.0)),
            new Data(new Tensor<double>(1.0, 0.0), new Tensor<double>(0.0)),
            new Data(new Tensor<double>(1.0, 1.0), new Tensor<double>(1.0))
        );

        public static Dataset NOT { get; } = new Dataset
        (
            new Data(new Tensor<double>(0.0), new Tensor<double>(1.0)),
            new Data(new Tensor<double>(1.0), new Tensor<double>(0.0))
        );
    }
}