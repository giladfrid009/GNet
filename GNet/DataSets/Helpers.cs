namespace GNet.Datasets
{
    public static class Helpers
    {
        public static bool VerifyDataset(Data[] dataset, int inputs, int outputs)
        {
            foreach (Data D in dataset)
            {
                if (D.Inputs.Length != inputs)
                    return false;

                if (D.Targets.Length != outputs)
                    return false;
            }

            return true;
        }
    }
}
