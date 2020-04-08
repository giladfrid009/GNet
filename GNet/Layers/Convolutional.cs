﻿using System;
using System.Collections.Generic;
using GNet.Model;
using GNet.Model.Convolutional;
using GNet.Utils;

namespace GNet.Layers
{
    [Serializable]
    public class Convolutional : ConvBase
    {
        public IActivation Activation { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }
        public override bool IsTrainable { get; set; } = true;

        public Convolutional(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, int nKernels, IActivation activation, IInitializer weightInit, IInitializer biasInit) :
            base(inputShape, kernelShape, strides, paddings, nKernels)
        {
            Activation = activation;
            WeightInit = weightInit;
            BiasInit = biasInit;
        }

        protected override Shape CalcOutputShape(Shape inputShape)
        {
            ArrayImmutable<int> channelDims = inputShape.Dimensions.Select((D, i) => 1 + (D + 2 * Paddings[i] - KernelShape.Dimensions[i]) / Strides[i]);

            return new Shape(new ArrayImmutable<int>(KernelsNum).Concat(channelDims));
        }

        public override void Connect(ILayer inLayer)
        {
            if (inLayer.Shape != InputShape)
            {
                throw new ArgumentException("InLayer shape mismatch.");
            }

            ShapedArrayImmutable<Neuron> padded = PadInNeurons(inLayer);

            var inConnections = new ShapedArrayImmutable<ArrayBuilder<Synapse>>(PaddedShape, () => new ArrayBuilder<Synapse>());

            Kernels.ForEach((kernel, i) =>
            {
                int offset = i * Shape.Volume / KernelsNum;

                IndexGen.ByStrides(PaddedShape, Strides, KernelShape).ForEach((idxKernel, j) =>
                {
                    var N = (CNeuron)Neurons[offset + j];

                    N.KernelBias = kernel.Bias;

                    N.InSynapses = IndexGen.ByStart(KernelShape, new ArrayImmutable<int>(idxKernel)).Select((idx, k) =>
                    {
                        var S = new CSynapse(padded[idx], N)
                        {
                            KernelWeight = kernel.Weights[k]
                        };

                        inConnections[idx].Add(S);
                        return (Synapse)S;
                    });
                });
            });

            padded.ForEach((N, i) => N.OutSynapses = inConnections[i].ToImmutable());
        }

        public override void Initialize()
        {
            int inLength = KernelShape.Volume;

            Kernels.ForEach(K =>
            {
                K.Bias.Value = BiasInit.Initialize(inLength, 1);
                K.Weights.ForEach(W => W.Value = WeightInit.Initialize(inLength, 1));
            });
        }

        public override void Forward()
        {
            Neurons.ForEach(N => N.Value = N.Bias + N.InSynapses.Sum(S => S.Weight * S.InNeuron.ActivatedValue));

            ShapedArrayImmutable<double> activated = Activation.Activate(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }

        public override void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ArgumentException("Targets shape mismatch.");
            }

            if (loss is IOutTransformer)
            {
                throw new ArgumentException($"{nameof(loss)} loss doesn't support backpropogation.");
            }

            ShapedArrayImmutable<double> actvDers = Activation.Derivative(Neurons.Select(N => N.Value));
            ShapedArrayImmutable<double> lossDers = loss.Derivative(targets, Neurons.Select(N => N.ActivatedValue));
            ShapedArrayImmutable<double> grads = lossDers.Combine(actvDers, (LD, AD) => LD * AD);

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = grads[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public override void CalcGrads()
        {
            ShapedArrayImmutable<double> actvDers = Activation.Derivative(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient) * actvDers[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public override void Update()
        {
            Neurons.ForEach(N =>
            {
                N.Bias += N.BatchDelta;
                N.BatchDelta = 0.0;

                N.InSynapses.ForEach(S =>
                {
                    S.Weight += S.BatchDelta;
                    S.BatchDelta = 0.0;
                });
            });
        }
    }
}