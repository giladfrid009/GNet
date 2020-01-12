using System;
using GNet.Extensions.IShapedArray;
using GNet.Extensions.IArray;
using GNet.GlobalRandom;

namespace GNet.Layers
{
    [Serializable]
    public class Dropout : Dense
    {
        public double DropChance { get; }

        private ShapedArrayImmutable<bool> dropArray;

        public Dropout(Shape shape, IActivation activation, IInitializer weightInit, IInitializer biasInit, double dropChance) : base(shape, activation, weightInit, biasInit)
        {
            if (dropChance < 0.0 || dropChance > 1.0)
            {
                throw new ArgumentOutOfRangeException("Drop Chance must be in range (0 - 1).");
            }

            DropChance = dropChance;
            dropArray = new ShapedArrayImmutable<bool>(OutputShape);
        }

        public override void Input(ShapedArrayImmutable<double> values)
        {
            if (values.Shape != InputShape)
            {
                throw new ArgumentOutOfRangeException("values shape mismatch.");
            }

            InNeurons.ForEach((N, i) => N.Value = dropArray[i] ? 0 : values[i]);

            ShapedArrayImmutable<double> activated = Activation.Activate(InNeurons.Select(N => N.Value));

            OutNeurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }

        public override void Forward()
        {
            InNeurons.ForEach((N, i) => N.Value = dropArray[i] ? 0 : N.Bias + N.InSynapses.Sum(W => W.Weight * W.InNeuron.ActivatedValue));

            ShapedArrayImmutable<double> activated = Activation.Activate(InNeurons.Select(N => N.Value));

            OutNeurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }

        public override void Update()
        {
            base.Update();

            dropArray = dropArray.Select(X => GRandom.NextDouble() < DropChance ? true : false);
        }        
    }
}
