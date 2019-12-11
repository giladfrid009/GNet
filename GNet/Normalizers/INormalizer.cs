using System;
using System.Collections.Generic;
using System.Text;

namespace GNet
{
    public interface INormalizer : ICloneable<INormalizer>
    {
        bool NormalizeInputs { get; set; }
        bool NormalizeOutputs { get; set; }

        void ExtractParams(Dataset dataset);

        double[] Normalize(double[] vals);
    }
}
