namespace GNet
{
    public interface INormalizer
    {
        void UpdateParams(Dataset dataset, bool inputs, bool targets);

        double Normalize(double X);
    }
}