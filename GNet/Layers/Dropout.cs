using GNet.Model;
using GNet.Utils;
using System;

namespace GNet.Layers
{
    [Serializable]
    public class Dropout : Reshape
    {
        public double DropChance { get; }
        public bool Spatial { get; }

        public Dropout(in Shape shape, double dropChance = 0.1, bool spatial = false) : base(shape)
        {
            if (dropChance < 0 || dropChance > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(dropChance));
            }

            DropChance = dropChance;
            Spatial = spatial;
        }

        public override void Initialize()
        {
            Update();

            base.Initialize();
        }

        public override void Forward(bool isTraining)
        {
            if (isTraining)
            {
                Neurons.ForEach((N, i) =>
                {
                    Synapse S = N.InSynapses[0];
                    N.InVal = S.Weight * S.InNeuron.OutVal;
                    N.OutVal = N.InVal;
                });
            }
            else
            {
                base.Forward(false);
            }
        }

        public override void Update()
        {
            if (Spatial)
            {
                int featureVol = Shape.Volume / Shape.Dims[0];
                bool drop = false;

                Neurons.ForEach((N, i) =>
                {
                    if (i % featureVol == 0)
                    {
                        drop = GRandom.Uniform(0, 1) <= DropChance;
                    }

                    N.InSynapses[0].Weight = drop ? 0.0 : 1.0;
                });
            }
            else
            {
                Neurons.ForEach(N => N.InSynapses[0].Weight = GRandom.Uniform(0, 1) <= DropChance ? 0.0 : 1.0);
            }
        }
    }
}