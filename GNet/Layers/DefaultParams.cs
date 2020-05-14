namespace GNet.Layers
{
    public static class DefaultParams
    {
        public static IInitializer WeightInit { get; set; }
        public static IInitializer BiasInit { get; set; }

        static DefaultParams()
        {
            WeightInit = new Initializers.GlorotUniform();
            BiasInit = new Initializers.Zero();
        }
    }
}
