﻿using NCollections;

namespace GNet
{
    public interface ILoss : IMetric
    {
        double Evaluate(double T, double O);

        double Derivative(double T, double O);

        double IMetric.Evaluate(Array<double> targets, Array<double> outputs)
        {
            return targets.Sum(outputs, (T, O) => Evaluate(T, O)) / targets.Length;
        }
    }
}