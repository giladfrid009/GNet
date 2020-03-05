﻿using GNet.Model;

namespace GNet
{
    public interface ILayer : ICloneable<ILayer>
    {
        ShapedArrayImmutable<Neuron> Neurons { get; }
        Shape Shape { get; }
        bool IsTrainable { get; }

        void Connect(ILayer inLayer);

        void Initialize();

        void Input(ShapedArrayImmutable<double> values);

        void Forward();

        void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets);

        void CalcGrads();

        void Update();

        void CopySynapses(ILayer layer);
    }
}