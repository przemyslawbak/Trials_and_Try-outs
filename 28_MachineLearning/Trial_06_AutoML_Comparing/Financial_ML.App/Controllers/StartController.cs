using Financial_ML.MachineLearning;
using Financial_ML.Models;
using Financial_ML.Services;
using Financial_ML.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;

namespace Financial_ML.App.Controllers
{
    public class StartController : Controller
    {
        private readonly IDataProvider _dataProvider;
        private readonly IDataTrimmer _dataTrimmer;
        private readonly IMlRegression _mlRegression;
        private readonly IMlBase _mlBase;
        private readonly List<RegressionMetrics> _metrixList;
        private readonly List<PredictionModel> _predictionList;

        public StartController(IDataProvider dataProvider, IDataTrimmer dataTrimmer, IMlRegression mlRegression, IMlBase mlBase)
        {
            _dataProvider = dataProvider;
            _dataTrimmer = dataTrimmer;
            _mlRegression = mlRegression;
            _mlBase = mlBase;
            _predictionList = new List<PredictionModel>();
            _metrixList = new List<RegressionMetrics>();
        }

        public IActionResult Index()
        {
            ResultsDisplay display = _dataProvider.GetResultsDisplayViewModel();
            MLContext context = _mlBase.GetMlContext();
            IDataView data = _mlBase.GetDataViewFromEnumerable(display.AllTotalQuotes, context);
            DataOperationsCatalog.TrainTestData trainTestData = _mlBase.GetTestData(context, data);
            RegressionExperimentSettings settings = new RegressionExperimentSettings
            {
                MaxExperimentTimeInSeconds = 60
            };
            RegressionExperiment experiment = context.Auto().CreateRegressionExperiment(settings);
            Progress<RunDetail<RegressionMetrics>> progress = new Progress<RunDetail<RegressionMetrics>>(p =>
            {
                if (p.ValidationMetrics != null)
                {
                    System.Diagnostics.Debug.Write($"Current result : {p.TrainerName}");
                    System.Diagnostics.Debug.Write($"      {p.ValidationMetrics.RSquared}");
                    System.Diagnostics.Debug.Write($"      {p.ValidationMetrics.MeanAbsoluteError}");
                    System.Diagnostics.Debug.WriteLine("");
                }
            });
            ExperimentResult<RegressionMetrics> result = experiment.Execute(trainTestData.TrainSet, labelColumnName: "NextDayCloseDax", progressHandler: progress);

            TotalQuote sample = new TotalQuote()
            {
                CloseBrent = 118.56F,
                CloseDax = 15216.27F,
                SmaBrent = 71.956F,
                SmaDax = 15461.68F,
                SmaDeltaBrent = 1,
                SmaDeltaDax = 1
            };

            return View();
        }
    }
}
