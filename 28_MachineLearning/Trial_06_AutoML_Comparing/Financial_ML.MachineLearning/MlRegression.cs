using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Financial_ML.MachineLearning
{
    public class MlRegression : IMlRegression
    {

        public EstimatorChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> GetRegressionPipeline(MLContext context, KeyValuePair<Type, object> algorithm)
        {
            return context.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "CloseDax")
                .Append(context.Transforms.Concatenate("Features", "CloseDax", "SmaDax", "SmaBrent", "CloseBrent", "SmaDeltaDax", "SmaDeltaBrent", "NextDayCloseDax"))
                .AppendCacheCheckpoint(context)
                .Append((IEstimator<RegressionPredictionTransformer<PoissonRegressionModelParameters>>)algorithm.Value);
        }
    }
}
