using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using System;

namespace Tutorial_06_TimeSeries
{
    class Program
    {
        static void Main(string[] args)
        {
            MLContext context = new MLContext();

            IDataView data = context.Data.LoadFromTextFile<EnergyData>("./energy_hourly.csv",
                hasHeader: true, separatorChar: ',');

            SsaForecastingEstimator pipeline = context.Forecasting.ForecastBySsa(
                nameof(EnergyForecast.Forecast),
                nameof(EnergyData.Energy),
                windowSize: 500,
                seriesLength: 1000,
                trainSize: 10000,
                horizon: 400);

            SsaForecastingTransformer model = pipeline.Fit(data);

            TimeSeriesPredictionEngine<EnergyData, EnergyForecast> forecastingEngine = model.CreateTimeSeriesEngine<EnergyData, EnergyForecast>(context);

            EnergyForecast forecasts = forecastingEngine.Predict();

            foreach (float forecast in forecasts.Forecast)
            {
                Console.WriteLine(forecast);
            }

            Console.ReadLine();
        }
    }
}
