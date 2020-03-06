namespace GNet.Layers
{
    public class Convolutional : Pooling
    {
        public IInitializer WeightInit { get; }

        public Convolutional(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IInitializer weightInit) : 
            base(inputShape, kernelShape, strides, paddings, new Kernels.Filter(weightInit))
        {
            WeightInit = weightInit.Clone();
        }

        public override void Update()
        {
            Neurons.ForEach(N =>
            {
                N.Bias += N.BatchBias;
                N.BatchBias = 0.0;

                N.InSynapses.ForEach(S =>
                {
                    S.Weight += S.BatchWeight;
                    S.BatchWeight = 0.0;
                });
            });
        }

        public override ILayer Clone()
        {
            return new Convolutional(InputShape, KernelShape, Strides, Paddings, WeightInit)
            {
                Kernel = Kernel.Clone(),
                Neurons = Neurons.Select(N => N.Clone())
            };
        }
    }
}