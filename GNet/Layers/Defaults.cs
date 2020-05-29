namespace GNet.Layers
{
    public static class Defaults
    {
        public static IInitializer WeightInit { get; set; } = new Initializers.GlorotUniform();
        public static IInitializer BiasInit { get; set; } = new Initializers.Zero();
    }
}