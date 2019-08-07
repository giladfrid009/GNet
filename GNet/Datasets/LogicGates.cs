namespace GNet.Datasets
{
    public static class LogicGates
    {
        public static Dataset AND => new Dataset(new Data[]
        {
            new Data(new double[] { 0.0, 0.0 }, new double[] { 0.0 }),
            new Data(new double[] { 0.0, 1.0 }, new double[] { 0.0 }),
            new Data(new double[] { 1.0, 0.0 }, new double[] { 0.0 }),
            new Data(new double[] { 1.0, 1.0 }, new double[] { 1.0 })
        });

        public static Dataset OR => new Dataset(new Data[]
        {
            new Data(new double[] { 0.0, 0.0 }, new double[] { 0.0 }),
            new Data(new double[] { 0.0, 1.0 }, new double[] { 1.0 }),
            new Data(new double[] { 1.0, 0.0 }, new double[] { 1.0 }),
            new Data(new double[] { 1.0, 1.0 }, new double[] { 1.0 })
        });

        public static Dataset XOR => new Dataset(new Data[]
        {
            new Data(new double[] { 0.0, 0.0 }, new double[] { 0.0 }),
            new Data(new double[] { 0.0, 1.0 }, new double[] { 1.0 }),
            new Data(new double[] { 1.0, 0.0 }, new double[] { 1.0 }),
            new Data(new double[] { 1.0, 1.0 }, new double[] { 0.0 })
        });

        public static Dataset NAND => new Dataset(new Data[]
        {
            new Data(new double[] { 0.0, 0.0 }, new double[] { 1.0 }),
            new Data(new double[] { 0.0, 1.0 }, new double[] { 1.0 }),
            new Data(new double[] { 1.0, 0.0 }, new double[] { 1.0 }),
            new Data(new double[] { 1.0, 1.0 }, new double[] { 0.0 })
        });

        public static Dataset NOR => new Dataset(new Data[]
        {
            new Data(new double[] { 0.0, 0.0 }, new double[] { 1.0 }),
            new Data(new double[] { 0.0, 1.0 }, new double[] { 0.0 }),
            new Data(new double[] { 1.0, 0.0 }, new double[] { 0.0 }),
            new Data(new double[] { 1.0, 1.0 }, new double[] { 0.0 })
        });

        public static Dataset XNOR => new Dataset(new Data[]
        {
            new Data(new double[] { 0.0, 0.0 }, new double[] { 1.0 }),
            new Data(new double[] { 0.0, 1.0 }, new double[] { 0.0 }),
            new Data(new double[] { 1.0, 0.0 }, new double[] { 0.0 }),
            new Data(new double[] { 1.0, 1.0 }, new double[] { 1.0 })
        });

        public static Dataset NOT => new Dataset(new Data[]
        {
            new Data(new double[] { 0.0 }, new double[] { 1.0 }),
            new Data(new double[] { 1.0 }, new double[] { 0.0 }),
        });
    }
}
