using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace Financial_ML.MachineLearning
{
    public interface IMlRegression
    {
        EstimatorChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> GetLbfgRegressionPipeline(MLContext context);
        RegressionMetrics GetRegressionMetrix(TransformerChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> modelRegression, MLContext context, DataOperationsCatalog.TrainTestData trainTestData);
    }
}