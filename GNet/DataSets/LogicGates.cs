namespace GNet.Datasets
{
    public static class LogicGates
    {
        public static readonly Data[] NOT = new Data[]
        {
            new Data(new double[] { 0 }, new double[] { 1 }),
            new Data(new double[] { 1 }, new double[] { 0 }),
        };

        public static readonly Data[] AND = new Data[]
        {
            new Data(new double[] { 0, 0 }, new double[] { 0 }),
            new Data(new double[] { 0, 1 }, new double[] { 0 }),
            new Data(new double[] { 1, 0 }, new double[] { 0 }),
            new Data(new double[] { 1, 1 }, new double[] { 1 })
        };

        public static readonly Data[] OR = new Data[]
        {
            new Data(new double[] { 0, 0 }, new double[] { 0 }),
            new Data(new double[] { 0, 1 }, new double[] { 1 }),
            new Data(new double[] { 1, 0 }, new double[] { 1 }),
            new Data(new double[] { 1, 1 }, new double[] { 1 })
        };

        public static readonly Data[] XOR = new Data[]
        {
            new Data(new double[] { 0, 0 }, new double[] { 0 }),
            new Data(new double[] { 0, 1 }, new double[] { 1 }),
            new Data(new double[] { 1, 0 }, new double[] { 1 }),
            new Data(new double[] { 1, 1 }, new double[] { 0 })
        };

        public static readonly Data[] NAND = new Data[]
        {
            new Data(new double[] { 0, 0 }, new double[] { 1 }),
            new Data(new double[] { 0, 1 }, new double[] { 1 }),
            new Data(new double[] { 1, 0 }, new double[] { 1 }),
            new Data(new double[] { 1, 1 }, new double[] { 0 })
        };

        public static readonly Data[] NOR = new Data[]
        {
            new Data(new double[] { 0, 0 }, new double[] { 1 }),
            new Data(new double[] { 0, 1 }, new double[] { 0 }),
            new Data(new double[] { 1, 0 }, new double[] { 0 }),
            new Data(new double[] { 1, 1 }, new double[] { 0 })
        };

        public static readonly Data[] NXOR = new Data[]
        {
            new Data(new double[] { 0, 0 }, new double[] { 1 }),
            new Data(new double[] { 0, 1 }, new double[] { 0 }),
            new Data(new double[] { 1, 0 }, new double[] { 0 }),
            new Data(new double[] { 1, 1 }, new double[] { 1 })
        };
    }
}
