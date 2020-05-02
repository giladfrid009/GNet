namespace GNet
{
    public interface INormalizer
    {
        void UpdateParams(Dataset dataArr, bool inputs, bool targets);

        double Normalize(double X);
    }
}