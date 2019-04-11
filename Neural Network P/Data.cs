using PNet.Extensions;

namespace PNet
{
    public class Data
    {
        public double[] Inputs { get; private set; }
        public double[] Targets { get; private set; }

        public Data(double[] inputs, double[] targets)
        {
            Inputs = inputs.CopyByVal();
            Targets = targets.CopyByVal();
        }
    }
}
