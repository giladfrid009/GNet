using System;

namespace GNet
{
    [Serializable]
    public abstract class TrainableObj
    {
        public double Gradient { get; set; }
        public double Cache1 { get; set; }
        public double Cache2 { get; set; }
        public double BatchDelta { get; set; }

        public void ClearCache()
        {
            Gradient = 0.0;
            Cache1 = 0.0;
            Cache2 = 0.0;
            BatchDelta = 0.0;
        }
    }
}