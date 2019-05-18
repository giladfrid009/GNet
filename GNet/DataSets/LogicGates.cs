namespace GNet.Datasets
{
    public static class LogicGates
    {
        public static Data[] NOT = new Data[] {
            new Data(new double[] { 0 }, new double[] { 1 }),
            new Data(new double[] { 1 }, new double[] { 0 }),
        };        

        public static Data[] AND = new Data[] {
            new Data(new double[] { 0, 0 }, new double[] { 0 }),
            new Data(new double[] { 0, 1 }, new double[] { 0 }),
            new Data(new double[] { 1, 0 }, new double[] { 0 }),
            new Data(new double[] { 1, 1 }, new double[] { 1 })
        };

        public static Data[] OR = new Data[] {
            new Data(new double[] { 0, 0 }, new double[] { 0 }),
            new Data(new double[] { 0, 1 }, new double[] { 1 }),
            new Data(new double[] { 1, 0 }, new double[] { 1 }),
            new Data(new double[] { 1, 1 }, new double[] { 1 })
        }; 

        public static Data[] XOR = new Data[] {
            new Data(new double[] { 0, 0 }, new double[] { 0 }),
            new Data(new double[] { 0, 1 }, new double[] { 1 }),
            new Data(new double[] { 1, 0 }, new double[] { 1 }),
            new Data(new double[] { 1, 1 }, new double[] { 0 })
        };

        public static Data[] NAND = new Data[] {
            new Data(new double[] { 0, 0 }, new double[] { 1 }),
            new Data(new double[] { 0, 1 }, new double[] { 1 }),
            new Data(new double[] { 1, 0 }, new double[] { 1 }),
            new Data(new double[] { 1, 1 }, new double[] { 0 })
        };

        public static Data[] NOR = new Data[] {
            new Data(new double[] { 0, 0 }, new double[] { 1 }),
            new Data(new double[] { 0, 1 }, new double[] { 0 }),
            new Data(new double[] { 1, 0 }, new double[] { 0 }),
            new Data(new double[] { 1, 1 }, new double[] { 0 })
        };

        public static Data[] NXOR = new Data[] {
            new Data(new double[] { 0, 0 }, new double[] { 1 }),
            new Data(new double[] { 0, 1 }, new double[] { 0 }),
            new Data(new double[] { 1, 0 }, new double[] { 0 }),
            new Data(new double[] { 1, 1 }, new double[] { 1 })
        };
    }
}
