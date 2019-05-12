using System.Collections.Generic;

namespace GNet.Datasets
{
    public static class LogicGates
    {
        public static List<Data> NOT = new List<Data>(new Data[] {
            new Data(new double[] { 0 }, new double[] { 1 }),
            new Data(new double[] { 1 }, new double[] { 0 }),
        });        

        public static List<Data> AND = new List<Data>(new Data[] {
            new Data(new double[] { 0, 0 }, new double[] { 0 }),
            new Data(new double[] { 0, 1 }, new double[] { 0 }),
            new Data(new double[] { 1, 0 }, new double[] { 0 }),
            new Data(new double[] { 1, 1 }, new double[] { 1 })
        });

        public static List<Data> OR = new List<Data>(new Data[] {
            new Data(new double[] { 0, 0 }, new double[] { 0 }),
            new Data(new double[] { 0, 1 }, new double[] { 1 }),
            new Data(new double[] { 1, 0 }, new double[] { 1 }),
            new Data(new double[] { 1, 1 }, new double[] { 1 })
        });        

        public static List<Data> XOR = new List<Data>(new Data[] {
            new Data(new double[] { 0, 0 }, new double[] { 0 }),
            new Data(new double[] { 0, 1 }, new double[] { 1 }),
            new Data(new double[] { 1, 0 }, new double[] { 1 }),
            new Data(new double[] { 1, 1 }, new double[] { 0 })
        });

        public static List<Data> NAND = new List<Data>(new Data[] {
            new Data(new double[] { 0, 0 }, new double[] { 1 }),
            new Data(new double[] { 0, 1 }, new double[] { 1 }),
            new Data(new double[] { 1, 0 }, new double[] { 1 }),
            new Data(new double[] { 1, 1 }, new double[] { 0 })
        });

        public static List<Data> NOR = new List<Data>(new Data[] {
            new Data(new double[] { 0, 0 }, new double[] { 1 }),
            new Data(new double[] { 0, 1 }, new double[] { 0 }),
            new Data(new double[] { 1, 0 }, new double[] { 0 }),
            new Data(new double[] { 1, 1 }, new double[] { 0 })
        });

        public static List<Data> NXOR = new List<Data>(new Data[] {
            new Data(new double[] { 0, 0 }, new double[] { 1 }),
            new Data(new double[] { 0, 1 }, new double[] { 0 }),
            new Data(new double[] { 1, 0 }, new double[] { 0 }),
            new Data(new double[] { 1, 1 }, new double[] { 1 })
        });
    }
}
