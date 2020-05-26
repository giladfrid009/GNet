﻿using static System.Math;

namespace GNet.Layers
{
    public class Softmax : Dense
    {
        public Softmax(Shape shape, IInitializer? weightInit = null, IInitializer? biasInit = null, IConstraint? weightConst = null, IConstraint? biasConst = null)
            : base(shape, new Activations.Identity(), weightInit, biasInit)
        {
        }

        public override void Input(ImmutableShapedArray<double> values, bool isTraining)
        {
            if (values.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(values));
            }

            double eSum = 0.0;

            Neurons.ForEach((N, i) =>
            {
                N.InVal = values[i];
                N.OutVal = Exp(N.InVal);
                eSum += N.OutVal;
            });

            Neurons.ForEach(N => N.OutVal /= eSum);
        }

        public override void Forward(bool isTraining)
        {
            double eSum = 0.0;

            Neurons.ForEach((N, i) =>
            {
                N.InVal = N.Bias + N.InSynapses.Sum(S => S.Weight * S.InNeuron.OutVal);
                N.OutVal = Exp(N.InVal);
                eSum += N.OutVal;
            });

            Neurons.ForEach(N => N.OutVal /= eSum);
        }

        public override void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            double gSum = 0.0;

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = loss.Derivative(targets[i], N.OutVal);
                gSum += N.Gradient * N.OutVal;
            });

            Neurons.ForEach(N =>
            {
                N.Gradient = N.OutVal * (N.Gradient - gSum);
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.OutVal);
            });
        }

        public override void CalcGrads()
        {
            double gSum = 0.0;

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient);
                gSum += N.Gradient * N.OutVal;
            });

            Neurons.ForEach(N =>
            {
                N.Gradient = N.OutVal * (N.Gradient - gSum);
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.OutVal);
            });
        }
    }
}