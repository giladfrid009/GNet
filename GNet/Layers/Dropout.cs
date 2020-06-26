using GNet.Utils;
using System;

namespace GNet.Layers
{
    [Serializable]
    public class Dropout : Reshape
    {
        public double DropChance { get; }

        private ImmutableArray<bool> dropArray;

        public Dropout(Shape shape, double dropChance = 0.1) : base(shape)
        {
            if (dropChance < 0 || dropChance > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(dropChance));
            }

            DropChance = dropChance;
            dropArray = new ImmutableArray<bool>();
        }

        public override void Connect(ILayer inLayer)
        {
            if (inLayer.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(inLayer));
            }

            base.Connect(inLayer);
        }

        public override void Input(ImmutableShapedArray<double> values, bool isTraining)
        {
            if (values.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(values));
            }

            if(isTraining)
            {
                Neurons.ForEach((N, i) =>
                {
                    N.InVal = values[i];
                    N.OutVal = dropArray[i] ? 0 : N.InVal;
                });
            }
            else
            {
                base.Input(values, false);
            }
        }

        public override void Forward(bool isTraining)
        {
            if(isTraining)
            {
                Neurons.ForEach((N, i) =>
                {
                    N.InVal = N.InVal = N.InSynapses[0].InNeuron.OutVal;
                    N.OutVal = N.OutVal = dropArray[i] ? 0 : N.InVal;
                });
            }
            else
            {
                base.Forward(false);
            }
        }

        public override void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            Neurons.ForEach((N, i) => N.Gradient = dropArray[i] ? 0 : loss.Derivative(targets[i], N.OutVal));
        }

        public override void CalcGrads()
        {
            Neurons.ForEach((N, i) => N.Gradient = dropArray[i] ? 0 : N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient));
        }

        public override void Update()
        {
            dropArray = new ImmutableArray<bool>(Shape.Volume, () => GRandom.Uniform(0, 1) <= DropChance);
        }
    }
}