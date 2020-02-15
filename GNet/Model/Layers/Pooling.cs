using System;
using GNet.Model;

namespace GNet.Layers
{

    [Serializable]
    public class Pooling : ILayer
    {
        public ShapedArrayImmutable<InNeuron> InNeurons { get; private set; }
        public ShapedArrayImmutable<OutNeuron> OutNeurons { get; private set; } // todo: assign value
        public ArrayImmutable<int> Strides { get; }
        public ArrayImmutable<int> Paddings { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }
        public Shape Kernel { get; }
        public Shape PaddedShape { get; }
        public IPooler Pooler { get; }

        // TODO: CACHE ALL GETINDICES!!

        public Pooling(Shape input, Shape kernel, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IPooler pooler)
        {
            ValidateParams(input, kernel, strides, paddings);

            InputShape = input;
            Kernel = kernel;
            Strides = strides;
            Paddings = paddings;
            Pooler = pooler.Clone();

            PaddedShape = CalcPaddedShape(input, paddings);

            OutputShape = CalcOutputShape(input, kernel, strides, paddings);

            InNeurons = new ShapedArrayImmutable<InNeuron>(input, () => new InNeuron());

            OutNeurons = new ShapedArrayImmutable<OutNeuron>(OutputShape, () => new OutNeuron());
        }

        private void ValidateParams(Shape input, Shape kernel, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
        {
            if (input.NumDimentions != kernel.NumDimentions)
            {
                throw new ArgumentOutOfRangeException("Kernel and input number of dimensions mismatch.");
            }

            if (input.NumDimentions != strides.Length)
            {
                throw new ArgumentOutOfRangeException("Strides and input number of dimensions mismatch.");
            }

            if (input.NumDimentions != paddings.Length)
            {
                throw new ArgumentOutOfRangeException("Pernel and input number of dimensions mismatch.");
            }

            for (int i = 0; i < input.NumDimentions; i++)
            {
                if (kernel.Dimensions[i] > input.Dimensions[i] || kernel.Dimensions[i] < 1)
                {
                    throw new ArgumentOutOfRangeException($"Kernel dimension [{i}] is out of range.");
                }

                if (strides[i] < 1)
                {
                    throw new ArgumentOutOfRangeException($"Strides [{i}] is out of range.");
                }

                if (strides[i] < 0)
                {
                    throw new ArgumentOutOfRangeException($"Paddings [{i}] is out of range.");
                }
            }
        }

        private Shape CalcOutputShape(Shape input, Shape kernel, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
        {
            ArrayImmutable<int> outputDims = input.Dimensions.Select((dim, i) => 1 + (dim + 2 * paddings[i] - kernel.Dimensions[i]) / strides[i]);

            return new Shape(outputDims.ToMutable());
        }

        private Shape CalcPaddedShape(Shape input, ArrayImmutable<int> paddings)
        {
            return new Shape(input.Dimensions.Select((D, i) => D + 2 * paddings[i]));
        }

        public void Initialize()
        {
            throw new NotSupportedException();
        }

        public void Connect(ILayer inLayer)
        {
            if (inLayer.OutputShape != InputShape)
            {
                throw new ArgumentException("InLayer shape volume mismatch.");
            }

            InNeurons.ForEach((N, i) =>
            {
                N.InSynapses = new ShapedArrayImmutable<Synapse>(new Shape(1), new Synapse(inLayer.OutNeurons[i], N));

                inLayer.OutNeurons[i].OutSynapses = N.InSynapses;
            });
        }

        private ShapedArrayImmutable<double> PadValues(ShapedArrayImmutable<double> values)
        {
            var padded = Array.CreateInstance(typeof(double), PaddedShape.Dimensions.ToMutable());

            InputShape.GetIndicesFrom(Paddings).ForEach((idx, i) => padded.SetValue(values[i], idx));

            return new ShapedArrayImmutable<double>(PaddedShape, padded);
        }

        public virtual void Input(ShapedArrayImmutable<double> values)
        {
            if (values.Shape != InputShape)
            {
                throw new ArgumentOutOfRangeException("values shape mismatch.");
            }

            InNeurons.ForEach((N, i) => N.Value = values[i]);

            ShapedArrayImmutable<double> padded = PadValues(values);

            PaddedShape.GetIndicesByStrides(Strides).ForEach((idxKernel, i) =>
            {
                ShapedArrayImmutable<double> kernel = Kernel.GetIndicesFrom(idxKernel).Select(idx => padded[idx]).ToShape(Kernel);
                OutNeurons[i].ActivatedValue = Pooler.Pool(kernel);
            });
        }

        public virtual void Forward()
        {
            InNeurons.ForEach(N => N.Value = N.InSynapses[0].InNeuron.ActivatedValue);

            ShapedArrayImmutable<double> padded = PadValues(InNeurons.Select(N => N.Value));

            PaddedShape.GetIndicesByStrides(Strides).ForEach((idxKernel, i) =>
            {
                ShapedArrayImmutable<double> kernel =  Kernel.GetIndicesFrom(idxKernel).Select(idx => padded[idx]).ToShape(Kernel);
                OutNeurons[i].ActivatedValue = Pooler.Pool(kernel);
            });
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

            ShapedArrayImmutable<double> lossDers = loss.Derivative(targets, OutNeurons.Select(N => N.ActivatedValue));

            OutNeurons.ForEach((N, i) => N.Gradient = lossDers[i]);

            // todo: what now

            throw new NotImplementedException();
        }

        public void CalcGrads()
        {
            // todo
            throw new NotImplementedException();
        }

        public void Update()
        {

        }            

        public ILayer Clone()
        {
            return new Pooling(InputShape, Kernel, Strides, Paddings, Pooler)
            {
                InNeurons = InNeurons.Select(N => N.Clone()),
                OutNeurons = OutNeurons.Select(N => N.Clone())
            };
        }
    }
}
