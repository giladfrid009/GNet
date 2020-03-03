using GNet.Layers.Kernels;

namespace GNet.Layers.Internal
{
    public class ConvOut : PoolingOut
    {
        public override bool IsTrainable { get; } = true;

        public ConvOut(Shape shapeInput, Shape shapeKernel, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, Filter kernel) : base(shapeInput, shapeKernel, strides, paddings, kernel)
        {

        }

        public override void Initialize()
        {
            Neurons.ForEach(N =>
            {
                N.Bias = 0;
                N.InSynapses.ForEach((S, i) => S.Weight = Kernel.Weights[i]);
            });
        }

        public override void Forward()
        {
            Neurons.ForEach(N =>
            {
                N.Value = N.InSynapses.Sum(S => S.Weight * S.InNeuron.ActivatedValue);
                N.ActivatedValue = N.Value;
            });
        }

        public override void Update()
        {
            Neurons.ForEach(N =>
            {
                N.BatchBias = 0.0;

                Kernel.Update(N.InSynapses);

                N.InSynapses.ForEach(S => S.BatchWeight = 0.0);
            });
        }

        public override ILayer Clone()
        {
            return new ConvOut(ShapeInput, ShapeKernel, Strides, Paddings, (Filter)Kernel)
            {
                Kernel = Kernel.Clone(),
                Neurons = Neurons.Select(N => N.Clone())
            };
        }
    }
}
