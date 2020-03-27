using System;

namespace GNet.Layers
{
    [Serializable]
    public class Dropout : Dense
    {
        // todo: idk if dropout works as intended. setting the value to 0 isn't enough, we need to set the weights leading to it to 0 also maybe?
        // todo: each layer should be able to implement dropout. maybe?

        public double DropChance { get; }

        private ShapedArrayImmutable<bool> dropArray;

        public Dropout(Shape shape, IActivation activation, IInitializer weightInit, IInitializer biasInit, double dropChance) : base(shape, activation, weightInit, biasInit)
        {
            if (dropChance < 0.0 || dropChance > 1.0)
            {
                throw new ArgumentOutOfRangeException("Drop Chance must be in range (0 - 1).");
            }

            DropChance = dropChance;
            dropArray = new ShapedArrayImmutable<bool>(shape);
        }

        public override void Input(ShapedArrayImmutable<double> values)
        {
            if (values.Shape != Shape)
            {
                throw new ArgumentOutOfRangeException("Values shape mismatch.");
            }

            Neurons.ForEach((N, i) => N.Value = dropArray[i] ? 0 : values[i]);

            ShapedArrayImmutable<double> activated = Activation.Activate(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }

        public override void Forward()
        {
            Neurons.ForEach((N, i) => N.Value = dropArray[i] ? 0 : N.Bias + N.InSynapses.Sum(W => W.Weight * W.InNeuron.ActivatedValue));

            ShapedArrayImmutable<double> activated = Activation.Activate(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }

        public override void Update()
        {
            base.Update();

            dropArray = dropArray.Select(X => GRandom.NextDouble() < DropChance ? true : false);
        }

        public override ILayer Clone()
        {
            return new Dropout(Shape, Activation, WeightInit, BiasInit, DropChance)
            {
                dropArray = dropArray,
                Neurons = Neurons.Select(N => N.Clone()),
                IsTrainable = IsTrainable
            };
        }
    }
}