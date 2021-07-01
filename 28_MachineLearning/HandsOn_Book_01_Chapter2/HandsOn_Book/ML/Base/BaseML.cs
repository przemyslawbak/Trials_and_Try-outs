using HandsOn_Book.Common;
using Microsoft.ML;
using System;
using System.IO;

namespace HandsOn_Book.ML.Base
{
    public class BaseML
    {
        protected static string ModelPath => Path.Combine(AppContext.BaseDirectory, Constants.MODEL_FILENAME);

        protected readonly MLContext MlContext;

        protected BaseML()
        {
            //Initializing the object with a specific seed value is needed to create more
            //consistent results during the testing component. Once a model is loaded, the seed value (or
            //lack thereof) does not affect the output.
            MlContext = new MLContext(2020);
        }
    }
}
