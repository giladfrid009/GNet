namespace GNet
{
    public interface IOptimizable
    {
        double Gradient { get; set; }
        double Cache1 { get; set; }
        double Cache2 { get; set; }
        double BatchDelta { get; set; }
    }
}