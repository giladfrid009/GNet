# GNet

GNet is user friendly, simply written and easily understood library for neural networks.  
The goal of this project was to create the simplest neural network library possible without sacrificing functionality.
Also, extensibility was taken into the mind, so it's very easy to implement your own additions to this library.


Let's take a look at a sample neural network, which trains the network to classify even and odd quantities.

```csharp
private static void Main()
{
    // Our dataset generator. Data dimensions are [2, 5]
    var datasetGenerator = new Datasets.Generators.EvenOdd(new Shape(2, 5), true);

    // Training dataset
    Dataset tDataset = datasetGenerator.Generate(5000);

    // Validation dataset
    Dataset vDataset = datasetGenerator.Generate(100);

    // Our network model
    var net = new Network
    (
        // An input layer. It has a shape of [2, 5] dimensions.
        new Layers.Dense(new Shape(2, 5), new Activations.Identity()),

        // A hidden layer. This layer uses default bias initializer
        new Layers.Dense(new Shape(10), new Activations.Sigmoid(), new Initializers.TruncNormal()),

        // Our Output layer. Softmax has it's own layer type since it's derivation is different.
        new Layers.Softmax(new Shape(2))
    );

    // Here, we use the logger class, which subscibes to certain network actions, and uses them to log the training.
    using (new Logger(net))
    {
        // Here we train the network. 
        // - We use the tDataset to train it
        // - The loss of our choice is Categorical CrossEntropy
        // - The optimizer we chose is AdaGradWindow
        // - We selected 10 as our batch size
        // - Maximum number of epoches is 1000
        // - The target error is 0.01
        // - We use vDataset to validate and evaluate out network
        // - Accuracy is the metric we chose to use. We can use losses as metrics as well.
        net.Train(tDataset, new Losses.Categorical.CrossEntropy(), new Optimizers.AdaGradWindow(), 
          10, 1000, 0.01, vDataset, new Metrics.Classification.Accuracy());
    }

    Console.ReadKey();
}
```
