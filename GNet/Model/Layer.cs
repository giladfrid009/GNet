using System;
using GNet.Extensions;

namespace GNet
{
    public class Layer
    {
        public readonly int Length;
        public readonly IActivation Activation;
        public readonly IInitializer WeightInit;
        public readonly IInitializer BiasInit;
        public Neuron[] Neurons = new Neuron[0];

        public Layer(int length, IActivation activation, IInitializer weightInit, IInitializer biasInit)
        {
            Length = length;
            Activation = activation.Clone();
            WeightInit = weightInit.Clone();
            BiasInit = biasInit.Clone();

            Neurons = new Neuron[Length];
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron();
            }
        }

        public void Connect(Layer inLayer)
        {
            inLayer.Neurons.ForEach(N => N.OutSynapses = new Synapse[this.Length]);
            this.Neurons.ForEach(N => N.InSynapses = new Synapse[inLayer.Length]);

            this.Neurons.ForEach((outN, i) =>
            {
                inLayer.Neurons.ForEach((inN, j) =>
                {
                    Synapse W = new Synapse(inN, outN);
                    inN.OutSynapses[i] = W;
                    outN.InSynapses[j] = W;
                });
            });
        }

        public void Init(Layer inLayer)
        {
            Connect(inLayer);

            Neurons.ForEach(N =>
            {
                N.Bias = BiasInit.Init(inLayer.Length, Length);
                N.InSynapses.ForEach(W => W.Weight = WeightInit.Init(inLayer.Length, Length));
            });
        }

        public void SetInput(double[] values)
        {
            if (values.Length != Length)
                throw new ArgumentOutOfRangeException("values length mismatch");

            double[] activated = Activation.Activate(values);

            Neurons.ForEach((N, i) =>
            {
                N.Value = values[i];
                N.ActivatedValue = activated[i];
            });
        }

        public void FeedForward()
        {
            Neurons.ForEach(N => N.Value = N.Bias + N.InSynapses.Sum(W => W.Weight * W.InNeuron.ActivatedValue));

            double[] activated = Activation.Activate(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }

        public void CalcGradients(ILoss loss, double[] targets)
        {
            double[] actvDers = Activation.Derivative(Neurons.Select(N => N.Value));
            double[] lossDers = loss.Derivative(targets, Neurons.Select(N => N.ActivatedValue));
            double[] grads = lossDers.Combine(actvDers, (LD, AD) => LD * AD);

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = grads[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public void CalcGradients()
        {
            double[] actvDers = Activation.Derivative(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(W => W.Weight * W.OutNeuron.Gradient) * actvDers[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public void Backprop(IOptimizer optimizer, ILoss loss, double[] targets)
        {
            CalcGradients(loss, targets);
            optimizer.Optimize(Neurons);
        }

        public void Backprop(IOptimizer optimizer)
        {
            CalcGradients();
            optimizer.Optimize(Neurons);
        }

        public void Update()
        {
            Neurons.ForEach(N =>
            {
                N.Bias += N.BatchBias;
                N.Gradient = default;
                N.BatchBias = default;

                N.InSynapses.ForEach(S =>
                {
                    S.Weight += S.BatchWeight;
                    S.Gradient = default;
                    S.BatchWeight = default;
                });
            });
        }
    }
}
