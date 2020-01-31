using System;
using System.Collections.Generic;

namespace GNet.Layers
{
    [Serializable]

    // todo: should inNeurons shape be padded, or padding should be applied on each feedForward pass?
    public class Pooling : ILayer
    {
        public ShapedArrayImmutable<InNeuron> InNeurons { get; } // padded
        public ShapedArrayImmutable<OutNeuron> OutNeurons { get; } // todo: assign value
        public ArrayImmutable<int> Strides { get; }
        public ArrayImmutable<int> Paddings { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }
        public Shape Kernel { get; }

        public Pooling(Shape input, Shape kernel, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
        {
            ValidateParams(input, kernel, strides, paddings);

            InputShape = input;
            Kernel = kernel;
            Strides = strides;
            Paddings = paddings;

            OutputShape = CalcOutputShape(input, kernel, strides, paddings);

            InNeurons = new ShapedArrayImmutable<InNeuron>(CalcPaddedShape(input, paddings), () => new InNeuron());

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

            int i = 0;

            foreach (int[] idx in IndexesByStart(InputShape, Paddings))
            {
                InNeurons[idx].InSynapses = new ShapedArrayImmutable<Synapse>(new Shape(1), new Synapse(inLayer.OutNeurons[i], InNeurons[idx]));

                inLayer.OutNeurons[i].OutSynapses = InNeurons[idx].InSynapses;

                i++;
            }
        }

        public virtual void Input(ShapedArrayImmutable<double> values)
        {
            if (values.Shape != InputShape)
            {
                throw new ArgumentOutOfRangeException("values shape mismatch.");
            }

            int i = 0;

            foreach (int[] idx in IndexesByStart(InputShape, Paddings))
            {
                InNeurons[idx].Value = values[i++];
            }

            // todo: then forward

            foreach (int[] idx in IndexesByStrides(InNeurons.Shape, Strides))
            {

            }

            throw new NotImplementedException();
        }

        public virtual void Forward()
        {
            // todo: probably there is a better way
            InNeurons.ForEach(N => N.Value = N.InSynapses.Length > 0 ? N.InSynapses[0].InNeuron.ActivatedValue : 0);

            // tdoo: forward

            throw new NotImplementedException();
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

            throw new NotImplementedException();
        }

        public void CalcGrads()
        {
            throw new NotImplementedException();
        }

        public virtual void Update()
        {
            throw new NotSupportedException();
        }

        private static IEnumerable<int[]> GenerateIndexes(Shape shape)
        {
            return GenerateIndexes(shape, new ArrayImmutable<int>(shape.NumDimentions, () => 0), new ArrayImmutable<int>(shape.NumDimentions, () => 1));
        }

        private static IEnumerable<int[]> IndexesByStart(Shape shape, ArrayImmutable<int> start)
        {
            return GenerateIndexes(shape, start, new ArrayImmutable<int>(shape.NumDimentions, () => 1));
        }

        private static IEnumerable<int[]> IndexesByStrides(Shape shape, ArrayImmutable<int> strides)
        {
            return GenerateIndexes(shape, new ArrayImmutable<int>(shape.NumDimentions, () => 0), strides);
        }

        private static IEnumerable<int[]> GenerateIndexes(Shape shape, ArrayImmutable<int> start, ArrayImmutable<int> strides)
        {
            if (strides.Length != shape.NumDimentions)
            {
                throw new ArgumentOutOfRangeException("Shape and strides length mismatch.");
            }

            if (start.Length != shape.NumDimentions)
            {
                throw new ArgumentOutOfRangeException("Shape and start length mismatch.");
            }

            int lastIndex = shape.NumDimentions - 1;

            return GenerateRecursive(new int[shape.NumDimentions], 0);

            IEnumerable<int[]> GenerateRecursive(int[] current, int dim)
            {
                int bound = start[dim] + shape.Dimensions[dim];

                if (dim == lastIndex)
                {
                    for (int i = start[dim]; i < bound; i += strides[dim])
                    {
                        current[dim] = i;
                        yield return current;
                    }
                }
                else
                {
                    for (int i = start[dim]; i < bound; i += strides[dim])
                    {
                        current[dim] = i;

                        foreach (int[] idx in GenerateRecursive(current, dim + 1))
                        {
                            yield return idx;
                        }
                    }
                }

                current[dim] = 0;
            }
        }
    }
}
