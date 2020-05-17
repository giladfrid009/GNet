<p align="center">
  <img src="https://github.com/giladfrid009/GNet/blob/master/GNet_Logo.png"/>
</p>

GNet is user friendly, simply written and easily understood library for neural networks.  
The goal of this project was to create the simplest neural network library possible without sacrificing functionality.
Also, extensibility was taken into the mind, so it's very easy to implement your own additions to this library.

## Features
* Activation Functions
* Weight Initialization Functions
* Classification and Regression Loss Functions
* Optimization Functions
* Different Layer Types
* Convolution Support
* ND Data Support
* Learning Rate Decay Functions
* Metrics
* Data Normalization Functions
* Built-in Datasets and Dynamic Dataset Generators
* Serialization Support
* Training Logger

## Examples
Let's train a simple neural network, which classifies even and odd quantities.

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
        // An input layer with [2, 5] dimensions
        new Layers.Dense(new Shape(2, 5), new Activations.Identity()),

        // A hidden layer
        new Layers.Dense(new Shape(10), new Activations.Sigmoid(), new Initializers.TruncNormal()),

        // Our Output layer
        new Layers.Softmax(new Shape(2))
    );

    // Here, we use the logger class, which subscibes to certain network actions, and uses them to log the training.
    using (new Logger(net))
    {
        // Here we train the network.        
        net.Train
        (
            tDataset,                                   // The training dataset
            new Losses.Categorical.CrossEntropy(),      // Training loss
            new Optimizers.AdaGradWindow(),             // Weight & Bias optimization algorithm
            10,                                         // Batch size
            1000,                                       // Training epoches
            0.01,                                       // Target error
            vDataset,                                   // Validation dataset
            new Metrics.Classification.Accuracy()       // Validation metric
        );
    }

    Console.ReadKey();
}
```
## License
This project uses the MIT license.
See [LICENSE](https://github.com/giladfrid009/GNet/blob/master/LICENSE) for more information.
