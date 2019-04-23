using System;
using GNet.Extensions;

namespace GNet
{
    public enum WeightInitializers { Zeroes, Ones, Uniform, Gaussian, He, Xavier };

    public static class WeightProvider
    {
        static Random rnd = new Random();

        public static void InitializeLayer(double[][][] weights, int layerIndex, WeightInitializers initializer)
        {
            switch (initializer)
            {
                case WeightInitializers.Zeroes:
                {                   
                    for (int j = 0; j < weights[layerIndex].Length; j++)
                    {
                        for (int k = 0; k < weights[layerIndex][j].Length; k++)
                        {
                            weights[layerIndex][j][k] = 0;
                        }
                    }                  

                    return;
                }                
                case WeightInitializers.Ones:
                {
                    for (int j = 0; j < weights[layerIndex].Length; j++)
                    {
                        for (int k = 0; k < weights[layerIndex][j].Length; k++)
                        {
                            weights[layerIndex][j][k] = 1;
                        }
                    }

                    return;
                }
                case WeightInitializers.Uniform:
                {                 
                    for (int j = 0; j < weights[layerIndex].Length; j++)
                    {
                        for (int k = 0; k < weights[layerIndex][j].Length; k++)
                        {
                            weights[layerIndex][j][k] = rnd.NextDouble(-1, 1);                               
                        }
                    }

                    return;
                }
                case WeightInitializers.Gaussian:
                {                    
                    for (int j = 0; j < weights[layerIndex].Length; j++)
                    {
                        for (int k = 0; k < weights[layerIndex][j].Length; k++)
                        {
                            weights[layerIndex][j][k] = rnd.NextGaussian();
                        }
                    }

                    return;
                }
                case WeightInitializers.He:
                {
                    // for ReLU
                    double prevLayer = weights[layerIndex].Length;

                    for (int j = 0; j < weights[layerIndex].Length; j++)
                    {
                        for (int k = 0; k < weights[layerIndex][j].Length; k++)
                        {
                            weights[layerIndex][j][k] = rnd.NextGaussian() * Math.Sqrt(2.0 / prevLayer);
                        }
                    }

                    return;
                }
                case WeightInitializers.Xavier:
                {              
                    // for Sinh
                    double prevLayer = weights[layerIndex].Length;

                    for (int j = 0; j < weights[layerIndex].Length; j++)
                    {
                        for (int k = 0; k < weights[layerIndex][j].Length; k++)
                        {
                            weights[layerIndex][j][k] = rnd.NextGaussian() * Math.Sqrt(1.0 / prevLayer);
                        }
                    }

                    return;
                }
            }
        }
    }
}
