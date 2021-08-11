using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Financial_ML.MachineLearning
{
    public class MlRegression : IMlRegression
    {
        public RegressionMetrics GetRegressionMetrix(TransformerChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> modelRegression, MLContext context, DataOperationsCatalog.TrainTestData trainTestData)
        {
            IDataView predictionsRegresion = modelRegression.Transform(trainTestData.TestSet);
            return context.Regression.Evaluate(predictionsRegresion, "Label", "NextDayCloseDax");
        }

        public EstimatorChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> GetRegressionPipeline(MLContext context, KeyValuePair<Type, object> algorithm)
        {
            Type t = algorithm.Key;
            Assembly info = typeof(int).Assembly;
            var dupa = algorithm.Value as t.GetType();
            return context.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "CloseDax")
                .Append(context.Transforms.Concatenate("Features", "CloseDax", "SmaDax", "SmaBrent", "CloseBrent", "SmaDeltaDax", "SmaDeltaBrent", "NextDayCloseDax"))
                .AppendCacheCheckpoint(context)
                .Append((algorithm.Key)algorithm.Value);
        }
    }
}
