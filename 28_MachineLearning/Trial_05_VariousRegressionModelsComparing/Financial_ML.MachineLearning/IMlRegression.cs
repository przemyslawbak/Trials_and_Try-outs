using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace Financial_ML.MachineLearning
{
    public interface IMlRegression
    {
        EstimatorChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> GetRegressionPipeline(MLContext context, object algorythim);
        RegressionMetrics GetRegressionMetrix(TransformerChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> modelRegression, MLContext context, DataOperationsCatalog.TrainTestData trainTestData);
    }
}