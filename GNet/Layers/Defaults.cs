namespace GNet.Layers
{
    public static class Defaults
    {
        public static IInitializer WeightInit { get; set; } = new Initializers.GlorotUniform();
        public static IInitializer BiasInit { get; set; } = new Initializers.Zero();
        public static IRegularizer? WeightReg { get; set; } = null;
        public static IRegularizer? BiasReg { get; set; } = null;
        public static IConstraint? WeightConst { get; set; } = null;
        public static IConstraint? BiasConst { get; set; } = null;
    }
}