using System.Diagnostics;
using static TorchSharp.torch.nn;
using TorchSharp;
using TorchSharp.Modules;


namespace Sample
{
    //https://github.com/dotnet/TorchSharp/tree/main/src/Examples
    //https://github.com/dotnet/TorchSharp/blob/main/src/Examples/CIFAR10.cs

    internal class Program
    {
        private static int _epochs = 8;
        private static int _trainBatchSize = 64;
        private static int _testBatchSize = 128;

        private readonly static int _logInterval = 25;
        private readonly static int _numClasses = 100;

        private readonly static int _timeout = 3600;    // One hour by default.

        static void Main(string[] args)
        {
            var datasetPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            torch.random.manual_seed(1);

            var device =
                // This worked on a GeForce RTX 2080 SUPER with 8GB, for all the available network architectures.
                // It may not fit with less memory than that, but it's worth modifying the batch size to fit in memory.
                torch.cuda.is_available() ? torch.CUDA :
                torch.mps_is_available() ? torch.MPS :
                torch.CPU;

            if (device.type != DeviceType.CPU)
            {
                _trainBatchSize *= 8;
                _testBatchSize *= 8;
                _epochs *= 16;
            }

            var modelName = args.Length > 0 ? args[0] : "AlexNet";
            var epochs = args.Length > 1 ? int.Parse(args[1]) : _epochs;
            var timeout = args.Length > 2 ? int.Parse(args[2]) : _timeout;

            Console.WriteLine();
            Console.WriteLine($"\tRunning {modelName} with CIFAR100 on {device.type.ToString()} for {epochs} epochs, terminating after {TimeSpan.FromSeconds(timeout)}.");
            Console.WriteLine();

            Console.WriteLine($"\tCreating the model...");

            //available models
            Module<torch.Tensor, torch.Tensor> model = new AlexNet(_numClasses, device: device); //convolutional neural network (CNN)
            //Module<torch.Tensor, torch.Tensor> model = new TransformerModel(_numClasses, device: device); //not available
            //Module<torch.Tensor, torch.Tensor> model = ResNet.ResNet18(_numClasses, device: device); //Deep Residual Learning
            //Module<torch.Tensor, torch.Tensor> model = new VGG(_numClasses, device: device); //convolutional neural network (CNN)
            //Module<torch.Tensor, torch.Tensor> model = new MobileNet(_numClasses, device); //not available

            //optional: modular NN models
            /*var modules = new List<(string, Module<Tensor, Tensor>)>();
            modules.Add(($"conv2d-first", Conv2d(3, 32, kernelSize: 3, stride: 1, padding: 1, bias: false)));
            modules.Add(($"bnrm2d-first", BatchNorm2d(32)));
            modules.Add(($"relu-first", ReLU()));
            MakeLayers(modules, 32);
            modules.Add(("avgpool", AvgPool2d(new long[] { 2, 2 })));
            modules.Add(("flatten", Flatten()));
            modules.Add(($"linear", Linear(planes[planes.Length - 1], numClasses)));
            layers = Sequential(modules);*/


            Console.WriteLine($"\tPreparing training and test data...");
            Console.WriteLine();

            var train_data = torchvision.datasets.CIFAR100(datasetPath, true, download: true); //downloading train data
            var test_data = torchvision.datasets.CIFAR100(datasetPath, true, download: true); //downloading test data

            using var train = new DataLoader(train_data, _trainBatchSize, device: device, shuffle: true);
            using var test = new DataLoader(test_data, _testBatchSize, device: device, shuffle: false);

            using (var optimizer = torch.optim.Adam(model.parameters(), 0.001))
            {

                Stopwatch totalSW = new Stopwatch();
                totalSW.Start();

                for (var epoch = 1; epoch <= epochs; epoch++)
                {
                    Stopwatch epchSW = new Stopwatch();
                    epchSW.Start();

                    Train(model, optimizer, torch.nn.NLLLoss(), train, epoch, _trainBatchSize, train_data.Count);
                    Test(model, torch.nn.NLLLoss(), test, test_data.Count);

                    epchSW.Stop();
                    Console.WriteLine($"Elapsed time for this epoch: {epchSW.Elapsed.TotalSeconds} s.");

                    if (totalSW.Elapsed.TotalSeconds > timeout) break;
                }

                totalSW.Stop();
                Console.WriteLine($"Elapsed training time: {totalSW.Elapsed} s.");
            }

            model.Dispose();
        }

        private static void Train(
            Module<torch.Tensor, torch.Tensor> model,
            torch.optim.Optimizer optimizer,
            Loss<torch.Tensor, torch.Tensor, torch.Tensor> loss,
            DataLoader dataLoader,
            int epoch,
            long batchSize,
            long size)
        {
            model.train();

            int batchId = 1;
            long total = 0;
            long correct = 0;

            Console.WriteLine($"Epoch: {epoch}...");

            using (var d = torch.NewDisposeScope())
            {

                foreach (var data in dataLoader)
                {

                    optimizer.zero_grad();

                    var target = data["label"];
                    var prediction = model.call(data["data"]);
                    var lsm = torch.nn.functional.log_softmax(prediction, 1);
                    var output = loss.call(lsm, target);

                    output.backward();

                    optimizer.step();

                    total += target.shape[0];

                    var predicted = prediction.argmax(1);
                    correct += predicted.eq(target).sum().ToInt64();

                    if (batchId % _logInterval == 0 || total == size)
                    {
                        Console.WriteLine($"\rTrain: epoch {epoch} [{total} / {size}] Loss: {output.ToSingle().ToString("0.000000")} | Accuracy: {((float)correct / total).ToString("0.000000")}");
                    }

                    batchId++;

                    d.DisposeEverything();
                }
            }
        }

        private static void Test(
            Module<torch.Tensor, torch.Tensor> model,
            Loss<torch.Tensor, torch.Tensor, torch.Tensor> loss,
            DataLoader dataLoader,
            long size)
        {
            model.eval();

            double testLoss = 0;
            long correct = 0;
            int batchCount = 0;

            using (var d = torch.NewDisposeScope())
            {

                foreach (var data in dataLoader)
                {

                    var target = data["label"];
                    var prediction = model.call(data["data"]);
                    var lsm = torch.nn.functional.log_softmax(prediction, 1);
                    var output = loss.call(lsm, target);

                    testLoss += output.ToSingle();
                    batchCount += 1;

                    var predicted = prediction.argmax(1);
                    correct += predicted.eq(target).sum().ToInt64();

                    d.DisposeEverything();
                }
            }

            Console.WriteLine($"\rTest set: Average loss {(testLoss / batchCount).ToString("0.0000")} | Accuracy {((float)correct / size).ToString("0.0000")}");
        }
    }
}