using System;
using GNet.Extensions.Generic;

namespace GNet
{
    public interface IDataset : ICloneable<IDataset>
    {
        Data[] DataCollection { get; }
        int DataLength { get; }
        int InputLength { get; }
        int TargetLength { get; }
    }
}

namespace GNet.Datasets.Static
{    
    public class LogicGates : IDataset
    {
        public enum Gates { AND, OR, XOR, NAND, NOR, XNOR }

        public Data[] DataCollection { get; private set; } = new Data[0];
        public Gates Gate { get; }
        public int DataLength { get; } = 4;
        public int InputLength { get; } = 2;
        public int TargetLength { get; } = 1;

        public LogicGates(Gates logicGate)
        {
            Gate = logicGate;

            switch (Gate)
            {
                case Gates.AND:
                {
                    DataCollection = new Data[]
                    {
                        new Data(new double[] { 0.0, 0.0 }, new double[] { 0.0 }),
                        new Data(new double[] { 0.0, 1.0 }, new double[] { 0.0 }),
                        new Data(new double[] { 1.0, 0.0 }, new double[] { 0.0 }),
                        new Data(new double[] { 1.0, 1.0 }, new double[] { 1.0 })
                    };
                    break;
                }

                case Gates.OR:
                {
                    DataCollection = new Data[]
                    {
                        new Data(new double[] { 0.0, 0.0 }, new double[] { 0.0 }),
                        new Data(new double[] { 0.0, 1.0 }, new double[] { 1.0 }),
                        new Data(new double[] { 1.0, 0.0 }, new double[] { 1.0 }),
                        new Data(new double[] { 1.0, 1.0 }, new double[] { 1.0 })
                    };
                    break;
                }

                case Gates.XOR:
                {
                    DataCollection = new Data[]
                    {
                        new Data(new double[] { 0.0, 0.0 }, new double[] { 0.0 }),
                        new Data(new double[] { 0.0, 1.0 }, new double[] { 1.0 }),
                        new Data(new double[] { 1.0, 0.0 }, new double[] { 1.0 }),
                        new Data(new double[] { 1.0, 1.0 }, new double[] { 0.0 })
                    };
                    break;
                }

                case Gates.NAND:
                {
                    DataCollection = new Data[]
                    {
                        new Data(new double[] { 0.0, 0.0 }, new double[] { 1.0 }),
                        new Data(new double[] { 0.0, 1.0 }, new double[] { 1.0 }),
                        new Data(new double[] { 1.0, 0.0 }, new double[] { 1.0 }),
                        new Data(new double[] { 1.0, 1.0 }, new double[] { 0.0 })
                    };
                    break;
                }

                case Gates.NOR:
                {
                    DataCollection = new Data[]
                    {
                        new Data(new double[] { 0.0, 0.0 }, new double[] { 1.0 }),
                        new Data(new double[] { 0.0, 1.0 }, new double[] { 0.0 }),
                        new Data(new double[] { 1.0, 0.0 }, new double[] { 0.0 }),
                        new Data(new double[] { 1.0, 1.0 }, new double[] { 0.0 })
                    };
                    break;
                }

                case Gates.XNOR:
                {
                    DataCollection = new Data[]
                    {
                        new Data(new double[] { 0.0, 0.0 }, new double[] { 1.0 }),
                        new Data(new double[] { 0.0, 1.0 }, new double[] { 0.0 }),
                        new Data(new double[] { 1.0, 0.0 }, new double[] { 0.0 }),
                        new Data(new double[] { 1.0, 1.0 }, new double[] { 1.0 })
                    };
                    break;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException("unsupported logic gate.");
                }
            }
        }      

        public IDataset Clone() => new LogicGates(Gate);
    }
}
