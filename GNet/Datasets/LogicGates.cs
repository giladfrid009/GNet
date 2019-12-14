namespace GNet.Datasets
{
    public static class LogicGates
    {
        public static Dataset AND { get; } = new Dataset(new Data[]
        {
            new Data(new ShapedArray<double>(new Shape(2), 0.0, 0.0), new ShapedArray<double>(new Shape(1), 0.0)),
            new Data(new ShapedArray<double>(new Shape(2), 0.0, 1.0), new ShapedArray<double>(new Shape(1), 0.0)),
            new Data(new ShapedArray<double>(new Shape(2), 1.0, 0.0), new ShapedArray<double>(new Shape(1), 0.0)),
            new Data(new ShapedArray<double>(new Shape(2), 1.0, 1.0), new ShapedArray<double>(new Shape(1), 1.0))
        });

        public static Dataset OR { get; } = new Dataset(new Data[]
        {
            new Data(new ShapedArray<double>(new Shape(2), 0.0, 0.0), new ShapedArray<double>(new Shape(1), 0.0)),
            new Data(new ShapedArray<double>(new Shape(2), 0.0, 1.0), new ShapedArray<double>(new Shape(1), 1.0)),
            new Data(new ShapedArray<double>(new Shape(2), 1.0, 0.0), new ShapedArray<double>(new Shape(1), 1.0)),
            new Data(new ShapedArray<double>(new Shape(2), 1.0, 1.0), new ShapedArray<double>(new Shape(1), 1.0))
        });

        public static Dataset XOR { get; } = new Dataset(new Data[]
        {
            new Data(new ShapedArray<double>(new Shape(2), 0.0, 0.0), new ShapedArray<double>(new Shape(1), 0.0)),
            new Data(new ShapedArray<double>(new Shape(2), 0.0, 1.0), new ShapedArray<double>(new Shape(1), 1.0)),
            new Data(new ShapedArray<double>(new Shape(2), 1.0, 0.0), new ShapedArray<double>(new Shape(1), 1.0)),
            new Data(new ShapedArray<double>(new Shape(2), 1.0, 1.0), new ShapedArray<double>(new Shape(1), 0.0))
        });

        public static Dataset NAND { get; } = new Dataset(new Data[]
        {
            new Data(new ShapedArray<double>(new Shape(2), 0.0, 0.0), new ShapedArray<double>(new Shape(1), 1.0)),
            new Data(new ShapedArray<double>(new Shape(2), 0.0, 1.0), new ShapedArray<double>(new Shape(1), 1.0)),
            new Data(new ShapedArray<double>(new Shape(2), 1.0, 0.0), new ShapedArray<double>(new Shape(1), 1.0)),
            new Data(new ShapedArray<double>(new Shape(2), 1.0, 1.0), new ShapedArray<double>(new Shape(1), 0.0))
        });

        public static Dataset NOR { get; } = new Dataset(new Data[]
        {
            new Data(new ShapedArray<double>(new Shape(2), 0.0, 0.0), new ShapedArray<double>(new Shape(1), 1.0)),
            new Data(new ShapedArray<double>(new Shape(2), 0.0, 1.0), new ShapedArray<double>(new Shape(1), 0.0)),
            new Data(new ShapedArray<double>(new Shape(2), 1.0, 0.0), new ShapedArray<double>(new Shape(1), 0.0)),
            new Data(new ShapedArray<double>(new Shape(2), 1.0, 1.0), new ShapedArray<double>(new Shape(1), 0.0))
        });

        public static Dataset XNOR { get; } = new Dataset(new Data[]
        {
            new Data(new ShapedArray<double>(new Shape(2), 0.0, 0.0), new ShapedArray<double>(new Shape(1), 1.0)),
            new Data(new ShapedArray<double>(new Shape(2), 0.0, 1.0), new ShapedArray<double>(new Shape(1), 0.0)),
            new Data(new ShapedArray<double>(new Shape(2), 1.0, 0.0), new ShapedArray<double>(new Shape(1), 0.0)),
            new Data(new ShapedArray<double>(new Shape(2), 1.0, 1.0), new ShapedArray<double>(new Shape(1), 1.0))
        });

        public static Dataset NOT { get; } = new Dataset(new Data[]
        {
            new Data(new ShapedArray<double>(new Shape(1), 0.0), new ShapedArray<double>(new Shape(1), 1.0)),
            new Data(new ShapedArray<double>(new Shape(1), 1.0), new ShapedArray<double>(new Shape(1), 0.0))
        });
    }
}
