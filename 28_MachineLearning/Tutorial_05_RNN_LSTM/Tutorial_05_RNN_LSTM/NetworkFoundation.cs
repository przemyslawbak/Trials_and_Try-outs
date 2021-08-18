using CNTK;
using NNetwork.Core.Common;

namespace Tutorial_05_RNN_LSTM
{
    class NetworkFoundation
    {
        public Variable Layer(Variable x, int outDim, DataType dataType,
                      DeviceDescriptor device, uint seed = 1, string name = "")
        {
            var b = Bias(outDim, dataType, device);
            var W = Weights(outDim, dataType, device, seed, name);

            var Wx = CNTKLib.Times(W, x, name + "_wx");
            var l = CNTKLib.Plus(b, Wx, name);

            return l;
        }

        public Parameter Bias(int nDimension, DataType dataType, DeviceDescriptor device)
        {
            //initial value
            var initValue = 0.01;
            NDShape shape = new int[] { nDimension };
            var b = new Parameter(shape, dataType, initValue, device, "_b");
            //
            return b;
        }

        public Parameter Weights(int nDimension, DataType dataType,
                                 DeviceDescriptor device, uint seed = 1, string name = "")
        {
            //initializer of parameter
            var glorotI = CNTKLib.GlorotUniformInitializer(1.0, 1, 0, seed);
            //create shape the dimension is partially known
            NDShape shape = new int[] { nDimension, NDShape.InferredDimension };
            var w = new Parameter(shape, dataType, glorotI, device, name == "" ? "_w" : name);
            //
            return w;
        }

        public Function AFunction(Variable x, Activation activation, string outputName = "")
        {
            switch (activation)
            {
                default:
                case Activation.None:
                    return x;
                case Activation.ReLU:
                    return CNTKLib.ReLU(x, outputName);
                case Activation.Softmax:
                    return CNTKLib.Sigmoid(x, outputName);
                case Activation.Tanh:
                    return CNTKLib.Tanh(x, outputName);
            }
        }
    }
}
