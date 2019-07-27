namespace GNet.Experimental
{
    // todo: implement everywhere possible? implement for network, layer, neuron and dataset?
    public interface IIndexable<Element>
    {
        int Length { get; }

        Element this[int index]
        {
            get;
        }
    }
}
