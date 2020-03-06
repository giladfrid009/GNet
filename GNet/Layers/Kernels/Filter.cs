using System;
using GNet.Model;

namespace GNet.Layers.Kernels
{
    [Serializable]
    public class Filter : IKernel
    {
        public IInitializer WeightInit { get; }
        public bool IsTrainable { get; set; } = true;

        public Filter(IInitializer weightInit)
        {
            WeightInit = weightInit.Clone();
        }        

        public ShapedArrayImmutable<double> InitWeights(ShapedArrayImmutable<double> inValues)
        {
            int nIn = inValues.Shape.Volume;
            return inValues.Select(X => WeightInit.Initialize(nIn, 1));
        }

        public IKernel Clone()
        {
            return new Filter(WeightInit)
            {
                IsTrainable = IsTrainable
            };
        }
    }
}