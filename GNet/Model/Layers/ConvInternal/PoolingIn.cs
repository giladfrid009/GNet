using GNet.Model;

namespace GNet.Layers.ConvInternal
{
    public class PoolingIn : Dense
    {
        public ArrayImmutable<int> Paddings { get; }
        public Shape ShapeInput { get; }

        public PoolingIn(Shape shapeInput, ArrayImmutable<int> paddings) : base(CalcPaddedShape(shapeInput, paddings), new Activations.Identity(), new Initializers.One(), new Initializers.Zero())
        {
            ShapeInput = shapeInput;
            Paddings = paddings;
        }

        private static Shape CalcPaddedShape(Shape input, ArrayImmutable<int> paddings)
        {
            return new Shape(input.Dimensions.Select((D, i) => D + 2 * paddings[i]));
        }

        public override void Connect(ILayer inLayer)
        {
            ShapeInput.GetIndicesFrom(Paddings).ForEach((idx, i) =>
            {
                var S = new Synapse(inLayer.Neurons[i], Neurons[idx]);
                Neurons[idx].InSynapses = new ShapedArrayImmutable<Synapse>(new Shape(1), S);
                inLayer.Neurons[i].OutSynapses = new ShapedArrayImmutable<Synapse>(new Shape(1), S);
            });
        }

        public override ILayer Clone()
        {
            return new PoolingIn(Shape, Paddings)
            {
                Neurons = Neurons.Select(N => N.Clone())
            };
        }
    }
}
