using System;
using GNet.Model;

// todo: rewrite
// todo: make sese with inNeurons and outNeurons.
namespace GNet.Layers
{
    [Serializable]
    public class Dense : ILayer
    {
        public ShapedArrayImmutable<InNeuron> InNeurons { get; private set; }
        public ShapedArrayImmutable<OutNeuron> OutNeurons { get; private set; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }
        public IActivation Activation { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }


        public Dense(Shape shape, IActivation activation, IInitializer weightInit, IInitializer biasInit)
        {
            InputShape = shape;
            OutputShape = shape;
            Activation = activation.Clone();
            WeightInit = weightInit.Clone();
            BiasInit = biasInit.Clone();
            InNeurons = new ShapedArrayImmutable<InNeuron>(shape, () => new InNeuron());
            OutNeurons = new ShapedArrayImmutable<OutNeuron>(shape, () => new OutNeuron());
        }

        public void Connect(ILayer inLayer)
        {
            InNeurons.ForEach(outN => outN.InSynapses = inLayer.OutNeurons.Select(inN => new Synapse(inN, outN)));

            inLayer.OutNeurons.ForEach((inN, i) => inN.OutSynapses = InNeurons.Select(outN => outN.InSynapses[i]));
        }

        public void Initialize()
        {
            int inLength = InNeurons[0].InSynapses.Length;
            int outLength = OutputShape.Volume;

            InNeurons.ForEach(N =>
            {
                N.Bias = BiasInit.Initialize(inLength, outLength);
                N.InSynapses.ForEach(W => W.Weight = WeightInit.Initialize(inLength, outLength));
            });
        }

        public virtual void Input(ShapedArrayImmutable<double> values)
        {
            if (values.Shape != InputShape)
            {
                throw new ArgumentOutOfRangeException("values shape mismatch.");
            }

            InNeurons.ForEach((N, i) => N.Value = values[i]);

            ShapedArrayImmutable<double> activated = Activation.Activate(InNeurons.Select(N => N.Value));

            OutNeurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }

        public virtual void Forward()
        {
            InNeurons.ForEach(N => N.Value = N.Bias + N.InSynapses.Sum(W => W.Weight * W.InNeuron.ActivatedValue));

            ShapedArrayImmutable<double> activated = Activation.Activate(InNeurons.Select(N => N.Value));

            OutNeurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }

        public void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
        {
            if (targets.Shape != OutputShape)
            {
                throw new ArgumentException("targets shape mismatch.");
            }

            if (loss is IOutTransformer)
            {
                throw new ArgumentException($"{nameof(loss)} loss doesn't support backpropogation.");
            }

            ShapedArrayImmutable<double> actvDers = Activation.Derivative(InNeurons.Select(N => N.Value));
            ShapedArrayImmutable<double> lossDers = loss.Derivative(targets, OutNeurons.Select(N => N.ActivatedValue));
            ShapedArrayImmutable<double> grads = lossDers.Combine(actvDers, (LD, AD) => LD * AD);

            InNeurons.ForEach((N, i) =>
            {
                N.Gradient = grads[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public void CalcGrads()
        {
            ShapedArrayImmutable<double> actvDers = Activation.Derivative(InNeurons.Select(N => N.Value));

            InNeurons.ForEach((N, i) =>
            {
                N.Gradient = OutNeurons[i].OutSynapses.Sum(W => W.Weight * W.OutNeuron.Gradient) * actvDers[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public virtual void Update()
        {
            InNeurons.ForEach(N =>
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
    }
}
