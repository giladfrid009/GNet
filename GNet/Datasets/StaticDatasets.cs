using System;

namespace GNet
{
    // todo: incorporate into the program.
    public interface IDataset : ICloneable<IDataset>
    {
        int Length { get; }
        int InputLength { get; }
        int OutputLength { get; }
        Data[] Dataset { get; }
    }
}

namespace GNet.Datasets.Static
{
    public class LogicGates : IDataset
    {
        public enum Gates { AND, OR, XOR }
        public Gates Gate { get; }

        public int Length { get; } = 4;
        public int InputLength { get; } = 2;
        public int OutputLength { get; } = 1;
        public Data[] Dataset { get; private set; } = new Data[0];

        public LogicGates(Gates logicGate)
        {
            Gate = logicGate;

            switch (Gate)
            {
                case Gates.AND:
                {
                    Dataset = new Data[]
                    {
                        new Data(new double[] { 0, 0 }, new double[] { 0 }),
                        new Data(new double[] { 0, 1 }, new double[] { 0 }),
                        new Data(new double[] { 1, 0 }, new double[] { 0 }),
                        new Data(new double[] { 1, 1 }, new double[] { 1 })
                    };
                    break;
                }
                case Gates.OR:
                {
                    Dataset = new Data[]
                    {
                        new Data(new double[] { 0, 0 }, new double[] { 0 }),
                        new Data(new double[] { 0, 1 }, new double[] { 1 }),
                        new Data(new double[] { 1, 0 }, new double[] { 1 }),
                        new Data(new double[] { 1, 1 }, new double[] { 1 })
                    };
                    break;
                }
                case Gates.XOR:
                {
                    Dataset = new Data[]
                    {
                        new Data(new double[] { 0, 0 }, new double[] { 0 }),
                        new Data(new double[] { 0, 1 }, new double[] { 1 }),
                        new Data(new double[] { 1, 0 }, new double[] { 1 }),
                        new Data(new double[] { 1, 1 }, new double[] { 0 })
                    };
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException("Unsupported Gate");
                }
            }
        }      

        public IDataset Clone() => new LogicGates(Gate);
    }
}
