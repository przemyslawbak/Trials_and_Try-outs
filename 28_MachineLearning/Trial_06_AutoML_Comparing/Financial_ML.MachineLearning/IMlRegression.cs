using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;

namespace Financial_ML.MachineLearning
{
    public interface IMlRegression
    {
        EstimatorChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> GetRegressionPipeline(MLContext context, KeyValuePair<Type, object> algorythim);
    }
}