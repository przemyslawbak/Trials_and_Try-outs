﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Numpy.Models;
using Python.Runtime;
using Torch;

namespace SimpleNeuralNetworkExample
{
    partial class Program
    {
        /// <summary>
        /// A fully-connected ReLU network with one hidden layer, trained to predict y from
        /// x by minimizing squared Euclidean distance.
        /// 
        /// This implementation uses the nn package from PyTorch to build the network.
        /// PyTorch autograd makes it easy to define computational graphs and take gradients,
        /// but raw autograd can be a bit too low-level for defining complex neural networks;
        /// this is where the nn package can help. The nn package defines a set of Modules,
        /// which you can think of as a neural network layer that has produces output from
        /// input and may have some trainable weights.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void LearnWithNnModules(Tensor x, Tensor y)
        {
            Console.WriteLine("Using NN Modules:");

            // N is batch size; D_in is input dimension;
            // H is hidden dimension; D_out is output dimension.
            var (N, D_in, H, D_out) = (64, 1000, 100, 10);

            var stopwatch = Stopwatch.StartNew();
            // Use the nn package to define our model as a sequence of layers. nn.Sequential
            // is a Module which contains other Modules, and applies them in sequence to
            // produce its output. Each Linear Module computes output from input using a
            // linear function, and holds internal Tensors for its weight and bias.
            var model = new torch.nn.Sequential(
                new torch.nn.Linear(D_in, H),
                new torch.nn.ReLU(),
                new torch.nn.Linear(H, D_out)
            );
            model.cuda(0);

            // The nn package also contains definitions of popular loss functions; in this
            // case we will use Mean Squared Error (MSE) as our loss function.
            var loss_fn = new torch.nn.MSELoss(reduction: "sum");
            loss_fn.cuda(0);

            var learning_rate = 1.0e-4;
            for (int t = 0; t <= 500; t++)
            {
                // Forward pass: compute predicted y by passing x to the model. Module objects
                // override the __call__ operator so you can call them like functions. When
                // doing so you pass a Tensor of input data to the Module and it produces
                // a Tensor of output data.
                var y_pred = model.Invoke(x).First();

                // Compute and print loss. We pass Tensors containing the predicted and true
                // values of y, and the loss function returns a Tensor containing the
                // loss.
                var loss = loss_fn.Invoke(y_pred, y).First();
                if (t % 20 == 0)
                    Console.WriteLine($"\tstep {t}: {loss.item<double>():F4}");

                // Zero the gradients before running the backward pass.
                model.zero_grad();

                // Backward pass: compute gradient of the loss with respect to all the learnable
                // parameters of the model. Internally, the parameters of each Module are stored
                // in Tensors with requires_grad=True, so this call will compute gradients for
                // all learnable parameters in the model.
                loss.backward();

                // Update the weights using gradient descent. Each parameter is a Tensor, so
                // we can access its gradients like we did before.
                Py.With(torch.no_grad(), _ =>
                {
                    foreach (var param in model.parameters())
                        param.isub(learning_rate * param.grad);
                });
            }

            stopwatch.Stop();
            Console.WriteLine($"\telapsed time: {stopwatch.Elapsed.TotalSeconds:F3} seconds\n");
        }

    }
}
