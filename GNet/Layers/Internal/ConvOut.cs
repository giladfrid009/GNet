using GNet.Layers.Kernels;

namespace GNet.Layers.Internal
{
    public class ConvOut : PoolingOut
    {
        public ConvOut(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, Filter kernel) : base(inputShape, kernelShape, strides, paddings, kernel)
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
            return new ConvOut(InputShape, KernelShape, Strides, Paddings, (Filter)Kernel)
            {
                Kernel = Kernel.Clone(),
                Neurons = Neurons.Select(N => N.Clone())
            };
        }
    }
}