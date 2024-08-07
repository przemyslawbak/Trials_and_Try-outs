﻿using Financial_ML.MachineLearning;
using Financial_ML.Models;
using Financial_ML.Services;
using Financial_ML.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Trainers.FastTree;
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

            //train
            DataOperationsCatalog.TrainTestData trainTestData = _mlBase.GetTestData(context, data);
            //todo: move to ML service
            var regressors = new Dictionary<Type ,object>();
            regressors.Add(typeof(FastForestRegressionTrainer), context.Regression.Trainers.FastForest());
            regressors.Add(typeof(FastTreeRegressionTrainer), context.Regression.Trainers.FastTree());
            regressors.Add(typeof(FastTreeTweedieTrainer), context.Regression.Trainers.FastTreeTweedie());
            regressors.Add(typeof(GamRegressionTrainer), context.Regression.Trainers.Gam());
            regressors.Add(typeof(OnlineGradientDescentTrainer), context.Regression.Trainers.OnlineGradientDescent());
            regressors.Add(typeof(LbfgsPoissonRegressionTrainer), context.Regression.Trainers.LbfgsPoissonRegression());
            regressors.Add(typeof(SdcaRegressionTrainer), context.Regression.Trainers.Sdca());

            foreach (KeyValuePair<Type, object> algorithm in regressors)
            {
                RunAlgorithm(trainTestData, context, algorithm, sample);
            }

            display = _dataTrimmer.TrimList(display, 50);

            return View(display);
        }

        private void RunAlgorithm(DataOperationsCatalog.TrainTestData trainTestData, MLContext context, KeyValuePair<Type, object> algorithm, TotalQuote sample)
        {
            EstimatorChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> pipeline = _mlRegression.GetRegressionPipeline(context, algorithm);
            ITransformer trainedModel = pipeline.Fit(trainTestData.TrainSet);
            _metrixList.Add(context.Regression.Evaluate(trainTestData.TestSet));
            PredictionEngine<TotalQuote, DaxChangeRegressionPrediction> predictionFunction = context.Model.CreatePredictionEngine<TotalQuote, DaxChangeRegressionPrediction>(trainedModel);
            _predictionList.Add(new PredictionModel() { Result = predictionFunction.Predict(sample), ModelName = nameof(algorithm.Key) });
        }
    }
}
