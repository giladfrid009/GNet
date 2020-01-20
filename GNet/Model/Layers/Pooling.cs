using System;
using System.Collections.Generic;
using GNet.Extensions.Array;
using GNet.Extensions.IArray;
using GNet.Extensions.IShapedArray;

namespace GNet.Layers
{
    [Serializable]
    public class Pooling : ILayer
    {
        public ShapedArrayImmutable<InNeuron> InNeurons { get; } // padded
        public ShapedArrayImmutable<OutNeuron> OutNeurons { get; } // todo: assign value
        public ArrayImmutable<int> Strides { get; }
        public ArrayImmutable<int> Paddings { get; }
        public Shape InputShape { get; }
        public Shape PaddedShape { get; }
        public Shape OutputShape { get; }
        public Shape Kernel { get; }

        public Pooling(Shape input, Shape kernel, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
        {
            ValidateParams(input, kernel, strides, paddings);

            InputShape = input;
            Kernel = kernel;
            Strides = strides;
            Paddings = paddings;

            PaddedShape = CalcPaddedShape(input, paddings);

            OutputShape = CalcOutputShape(input, kernel, strides, paddings);

            InNeurons = new ShapedArrayImmutable<InNeuron>(input, () => new InNeuron());

            OutNeurons = new ShapedArrayImmutable<OutNeuron>(OutputShape, () => new OutNeuron());
        }

        private void ValidateParams(Shape input, Shape kernel, IArray<int> strides, IArray<int> paddings)
        {           
            if(input.NumDimentions != kernel.NumDimentions)
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
                if(kernel.Dimensions[i] > input.Dimensions[i] || kernel.Dimensions[i] < 1)
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

        private Shape CalcOutputShape(Shape input, Shape kernel, IArray<int> strides, IArray<int> paddings)
        {
            ArrayImmutable<int> outputDims = input.Dimensions.Select((dim, i) => 1 + (dim + 2 * paddings[i] - kernel.Dimensions[i]) / strides[i]);

            return new Shape(outputDims.ToMutable());
        }

        private Shape CalcPaddedShape(Shape input, IArray<int> paddings)
        {
            return new Shape(input.Dimensions.Select((D, i) => D + 2 * paddings[i]));
        }

        private ShapedArray<double> PadValues(IShapedArray<double> values)
        {
            var padded = new ShapedArray<double>(PaddedShape);

            int idxFlat = 0;

            foreach (int[] idx in GenerateIndexes(InputShape.Dimensions, Paddings, new ArrayImmutable<int>(InputShape.NumDimentions, () => 1)))
            {
                padded[idx] = values[idxFlat++];
            }

            return padded;
        }

        public void Initialize()
        {
            throw new NotSupportedException();
        }

        public void Connect(ILayer inLayer)
        {
            if(inLayer.OutputShape != InputShape)
            {
                throw new ArgumentException("InLayer shape volume mismatch.");
            }

            InNeurons.ForEach((outN, i) => outN.InSynapses = new ShapedArrayImmutable<Synapse>(new Shape(1), new Synapse(inLayer.OutNeurons[i], outN)));

            inLayer.OutNeurons.ForEach((inN, i) => inN.OutSynapses = InNeurons[i].InSynapses);
        }

        public virtual void Input(ShapedArrayImmutable<double> values)
        {
            if (values.Shape != InputShape)
            {
                throw new ArgumentOutOfRangeException("values shape mismatch.");
            }

            InNeurons.ForEach((N, i) => N.Value = values[i]);



            throw new NotImplementedException();
        }

        public virtual void Forward()
        {
            InNeurons.ForEach(N => N.Value = N.Bias + N.InSynapses.Sum(W => W.Weight * W.InNeuron.ActivatedValue));

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

        static IEnumerable<int[]> GenerateIndexes(IArray<int> ranges)
        {
            return GenerateIndexes(ranges, new ArrayImmutable<int>(ranges.Length, () => 0), new ArrayImmutable<int>(ranges.Length, () => 1));
        }

        static IEnumerable<int[]> GenerateIndexes(IArray<int> ranges, IArray<int> start, IArray<int> strides)
        {
            if(strides.Length != ranges.Length)
            {
                throw new ArgumentOutOfRangeException("Ranges and strides length mismatch.");
            }

            if(start.Length != ranges.Length)
            {
                throw new ArgumentOutOfRangeException("Ranges and start length mismatch.");
            }

            return GenerateRecursive(new int[ranges.Length], 0);

            IEnumerable<int[]> GenerateRecursive(int[] current, int dim)
            {
                int bound = start[dim] + ranges[dim];

                if (dim == ranges.Length - 1)
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

                        foreach (int[] idx in GenerateRecursive(current, dim+1))
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
