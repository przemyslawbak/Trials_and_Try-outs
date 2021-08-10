using Financial_ML.MachineLearning;
using Financial_ML.Models;
using Financial_ML.Services;
using Financial_ML.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
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

            //train
            DataOperationsCatalog.TrainTestData trainTestData = _mlBase.GetTestData(context, data);

            //regression pipeline
            EstimatorChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> pipelineRegression = _mlRegression.GetRegressionPipeline(context);
            TransformerChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> modelRegression = pipelineRegression.Fit(trainTestData.TrainSet);

            //evaluate
            RegressionMetrics metricsRegresion = _mlRegression.GetRegressionMetrix(modelRegression, context, trainTestData);
            //prediction
            TransformerChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> model = pipelineRegression.Fit(trainTestData.TrainSet);
            PredictionEngine<TotalQuote, DaxChangeRegressionPrediction> predictionFunction = context.Model.CreatePredictionEngine<TotalQuote, DaxChangeRegressionPrediction>(model);
            TotalQuote sample = new TotalQuote()
            {
                CloseBrent = 118.56F,
                CloseDax = 15216.27F,
                Date = DateTime.Now.AddDays(-12),
                SmaBrent = 71.956F,
                SmaDax = 15461.68F,
                SmaDeltaBrent = 1,
                SmaDeltaDax = 1
            };
            DaxChangeRegressionPrediction predictionRegression = predictionFunction.Predict(sample); //shitty result

            display = _dataTrimmer.TrimList(display, 50);

            return View(display);
        }
    }
}
