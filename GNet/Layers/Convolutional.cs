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
            return new Convolutional(InputShape, KernelShape, Strides, Paddings, WeightInit)
            {
                Kernel = Kernel.Clone(),
                Neurons = Neurons.Select(N => N.Clone())
            };
        }
    }
}