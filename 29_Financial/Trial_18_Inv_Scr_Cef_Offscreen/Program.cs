using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.OffScreen
{
    public class Program
    {
        private static ChromiumWebBrowser _browser;
        private static List<DataObject> _allValues = new List<DataObject>();
        private static RequestContext _requestContext;
        private static BrowserSettings _browserSettings;
        private static EventHandler<LoadingStateChangedEventArgs> _pageLoadedEventHandler;

        static void Main(string[] args)
        {
            string itemSearched = "SPX_F";

            try
            {
                Task t = new Task(ExecuteProgram);
                t.Start();
                Cef.Shutdown();
                Console.ReadKey();
            }
            catch
            {

            }
            finally
            {
                var toSave = _allValues.GroupBy(x => x.Tick).Select(z => z.OrderBy(y => y.UtcTimeStamp).First()).ToList();
                File.WriteAllLines("_" + itemSearched + ".txt", toSave.Select(x => x.UtcTimeStamp + "|" + x.DataValue + "|" + x.Tick));

                Console.WriteLine("Saved");
            }
        }

        public static bool LoadingPage { get; set; }
        public static bool InitBrowser { get; set; }

        private static async void ExecuteProgram()
        {
            _requestContext = CreateContext();
            _browser = CreateBrowser();

            InitBrowser = true;
            _browser.BrowserInitialized += OnIsBrowserInitializedChangedAsync;

            while (InitBrowser)
            {
                await Task.Delay(100);
            }

            await RunProgramAsync();
        }

        private static void OnIsBrowserInitializedChangedAsync(object sender, EventArgs e)
        {
            InitBrowser = false;
        }

        private static ChromiumWebBrowser CreateBrowser()
        {
            _browserSettings = new BrowserSettings()
            {
                ImageLoading = CefState.Disabled
            };

            return new ChromiumWebBrowser("", _browserSettings, _requestContext, true);
        }

        private static RequestContext CreateContext()
        {
            using (CefSettings settings = new CefSettings())
            {
                settings.LogSeverity = LogSeverity.Disable;
                settings.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache");
                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            }

            RequestContextSettings requestContextSettings = new RequestContextSettings();

            requestContextSettings.PersistSessionCookies = false;
            requestContextSettings.PersistUserPreferences = false;

            return new RequestContext(requestContextSettings);
        }

        private static async Task RunProgramAsync()
        {
            
            await LoadPage();
        }

        private static async Task LoadPage()
        {
            LoadingPage = true;

            int spread = 199980 * 3;
            UrlVault urlVault = new UrlVault();
            var maxToTick = urlVault.GetToMaxInt();
            var toTick = maxToTick;
            var fromTick = toTick - spread;
            var dtMaxToTick = urlVault.GetToMaxIntUtcTimeStampe();
            var minTick = urlVault.GetToMinInt();

            while (fromTick > minTick)
            {
                string url = urlVault.GetBaseUrl() + urlVault.GetResolution() + "&from=" + fromTick + "&to=" + toTick;
                Console.WriteLine("processing from " + fromTick + " to " + toTick);
                _browser.Load(url);

                _pageLoadedEventHandler = (sender, args) =>
                {
                    if (args.IsLoading == false)
                    {
                        _browser.LoadingStateChanged -= _pageLoadedEventHandler;

                        LoadingPage = false;
                    }
                };

                _browser.LoadingStateChanged += _pageLoadedEventHandler;

                while (LoadingPage)
                {
                    await Task.Delay(100);
                }

                toTick = await GetAndSaveData(dtMaxToTick, maxToTick);
                fromTick = toTick - spread;

                await Task.Delay(2000);
            }
         }

        private static async Task<int> GetAndSaveData(DateTime dtMaxToTick, int maxTo)
        {
            var toReturn = 0;
            var dataValues = new List<DataObject>();
            string html = "";
            await _browser.GetSourceAsync().ContinueWith(taskHtml =>
            {
                html = taskHtml.Result;
            });

            var ticks = html
                    .Split(new string[] { "<html><head></head><body>{\"t\":[" }, StringSplitOptions.None)[1]
                    .Split(new string[] { "],\"c\":[" }, StringSplitOptions.None)[0]
                    .Split(',')
                    .Select(x => int.Parse(x))
                    .Reverse()
                    .ToList();

            var values = html
                .Split(new string[] { "],\"c\":[" }, StringSplitOptions.None)[1]
                .Split(new string[] { "],\"o\":[" }, StringSplitOptions.None)[0]
                .Split(',')
                .Select(x => decimal.Parse(x, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture))
                .Reverse()
                .ToList();

            for (int i = 0; i < ticks.Count; i++)
            {
                var multiplier = (maxTo - ticks[i]) / 60;
                dataValues.Add(new DataObject()
                {
                    DataValue = values[i],
                    UtcTimeStamp = dtMaxToTick.AddMinutes(-multiplier),
                    Tick = ticks[i],
                }); ;
            }

            _allValues.AddRange(dataValues);
            toReturn = ticks.Last();

            return toReturn;

        }
    }
}
