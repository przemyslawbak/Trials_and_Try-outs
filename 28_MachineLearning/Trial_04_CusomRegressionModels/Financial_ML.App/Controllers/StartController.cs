using Financial_ML.MachineLearning;
using Financial_ML.Models;
using Financial_ML.Services;
using Financial_ML.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Calibrators;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Trainers.LightGbm;
using System;

namespace Financial_ML.App.Controllers
{
    public class StartController : Controller
    {
        private readonly IDataProvider _dataProvider;
        private readonly IDataTrimmer _dataTrimmer;
        private readonly IMlRegression _mlRegression;
        private readonly IMlBase _mlBase;

        public StartController(IDataProvider dataProvider, IDataTrimmer dataTrimmer, IMlRegression mlRegression, IMlBase mlBase)
        {
            _dataProvider = dataProvider;
            _dataTrimmer = dataTrimmer;
            _mlRegression = mlRegression;
            _mlBase = mlBase;
        }

        public IActionResult Index()
        {
            ResultsDisplay display = _dataProvider.GetResultsDisplayViewModel();
            MLContext context = _mlBase.GetMlContext();
            IDataView data = _mlBase.GetDataViewFromEnumerable(display.AllTotalQuotes, context);
            DataOperationsCatalog.TrainTestData trainTestData = _mlBase.GetTestData(context, data);
            TotalQuote sampleForPrediction = new TotalQuote()
            {
                CloseBrent = 118.56F,
                CloseDax = 15216.27F,
                Date = DateTime.Now.AddDays(-12),
                SmaBrent = 71.956F,
                SmaDax = 15461.68F,
                SmaDeltaBrent = 1,
                SmaDeltaDax = 1
            };
            DaxChangeRegressionPrediction prediction = new DaxChangeRegressionPrediction();

            //LbfgsPoissonRegression
            EstimatorChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> pipelineLbfgRegression = _mlRegression.GetLbfgRegressionPipeline(context);
            TransformerChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> modelRegression = pipelineLbfgRegression.Fit(trainTestData.TrainSet);
            //evaluate
            RegressionMetrics metricsLbfgRegresion = _mlRegression.GetRegressionMetrix(modelRegression, context, trainTestData);
            //prediction
            TransformerChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> modelLbfg = pipelineLbfgRegression.Fit(trainTestData.TrainSet);
            PredictionEngine<TotalQuote, DaxChangeRegressionPrediction> predictionA = context.Model.CreatePredictionEngine<TotalQuote, DaxChangeRegressionPrediction>(modelLbfg);
            prediction = predictionA.Predict(sampleForPrediction);

            //LightGbmBinaryTrainer
            LightGbmBinaryTrainer.Options options = new LightGbmBinaryTrainer.Options
            {
                Booster = new GossBooster.Options
                {
                    TopRate = 0.3,
                    OtherRate = 0.2
                }
            };
            var pipeline = context.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "NextDayCloseDaxBoolean")
                .Append(context.Transforms.Concatenate("Features", "CloseDax", "SmaDax", "SmaBrent", "CloseBrent", "SmaDeltaDax", "SmaDeltaBrent", "NextDayCloseDax"))
                .Append(context.BinaryClassification.Trainers.LightGbm(options));

            TransformerChain<BinaryPredictionTransformer<CalibratedModelParametersBase<LightGbmBinaryModelParameters, PlattCalibrator>>> modelBinaryLightGmb = pipeline.Fit(data);
            //evaluate
            IDataView predictionsBinary = modelBinaryLightGmb.Transform(trainTestData.TestSet);
            CalibratedBinaryClassificationMetrics metricsBinary = context.BinaryClassification.Evaluate(predictionsBinary, "Label", "NextDayCloseDax");
            //prediction
            PredictionEngine<TotalQuote, DaxChangeRegressionPrediction> predictionB = context.Model.CreatePredictionEngine<TotalQuote, DaxChangeRegressionPrediction>(modelBinaryLightGmb);
            prediction = predictionB.Predict(sampleForPrediction);


            display = _dataTrimmer.TrimList(display, 50);

            return View(display);
        }
    }
}
