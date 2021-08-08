using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System.Linq;

namespace Financial_ML.MachineLearning
{
    public class MlRegression : IMlRegression
    {
        public RegressionMetrics GetRegressionMetrix(TransformerChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> modelRegression, MLContext context, DataOperationsCatalog.TrainTestData trainTestData)
        {
            IDataView predictionsRegresion = modelRegression.Transform(trainTestData.TestSet);
            return context.Regression.Evaluate(predictionsRegresion, "Label", "NextDayCloseDax");
        }

        public EstimatorChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> GetLbfgRegressionPipeline(MLContext context)
        {
            return context.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "CloseDax")
                .Append(context.Transforms.Concatenate("Features", "CloseDax", "SmaDax", "SmaBrent", "CloseBrent", "SmaDeltaDax", "SmaDeltaBrent", "NextDayCloseDax"))
                .Append(context.Regression.Trainers.LbfgsPoissonRegression());
        }
    }
}
