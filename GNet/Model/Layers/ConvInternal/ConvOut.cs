using GNet.Model;

namespace GNet.Layers.ConvInternal
{
    public class ConvOut : Dense
    {
        public ArrayImmutable<int> Strides { get; }
        public Shape ShapeInput { get; }
        public Shape ShapeKernel { get; }

        public ConvOut(Shape shapeInput, Shape shapeKernel, ArrayImmutable<int> strides) : base(CalcOutputShape(shapeInput, shapeKernel, strides), new Activations.Identity(), new Initializers.One(), new Initializers.Zero())
        {
            ShapeInput = shapeInput;
            ShapeKernel = shapeKernel;
            Strides = strides;
        }

        private static Shape CalcOutputShape(Shape padded, Shape kernel, ArrayImmutable<int> strides)
        {
            ArrayImmutable<int> outDims = padded.Dimensions.Select((D, i) => 1 + (D - kernel.Dimensions[i]) / strides[i]);

            return new Shape(outDims);
        }

        public override void Connect(ILayer inLayer)
        {
            ShapeInput.GetIndicesByStrides(Strides).ForEach((idxKernel, i) =>
            {
                Neurons[i].InSynapses = ShapeKernel.GetIndicesFrom(idxKernel).Select(idx => new Synapse(inLayer.Neurons[idx], Neurons[i])).ToShape(ShapeKernel);

            });
        }

        public override ILayer Clone()
        {
            return new ConvOut(Shape, ShapeKernel, Strides)
            {
                Neurons = Neurons.Select(N => N.Clone())
            };
        }
    }
}
