using CefSharp.OffScreen;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.OffScreen
{
    public class Program
    {
        private static ChromiumWebBrowser _browser;
        private static RequestContext _requestContext;
        private static BrowserSettings _browserSettings;
        private static EventHandler<LoadingStateChangedEventArgs> _pageLoadedEventHandler;

        static void Main(string[] args)
        {
            Task t = new Task(ExecuteProgram);
            t.Start();
            Console.ReadKey();
            Cef.Shutdown();
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
                settings.LogSeverity = LogSeverity.Disable; //disabled console logs
                settings.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"); //browser cache
                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null); //init
            }

            RequestContextSettings requestContextSettings = new RequestContextSettings();

            requestContextSettings.PersistSessionCookies = false; //separated cookies for each instance
            requestContextSettings.PersistUserPreferences = false; //separated user cred for each instance

            return new RequestContext(requestContextSettings);
        }

        private static async Task RunProgramAsync()
        {
            
            await LoadPage();

            Console.WriteLine("Finito");
        }

        private static async Task LoadPage()
        {
            LoadingPage = true;

            int spread = 199980;
            UrlVault urlVault = new UrlVault();
            string itemSearched = "DXY";
            var to = urlVault.GetToMaxInt();
            var from = to - spread;
            var dt = urlVault.GetToMaxIntUtcTimeStampe();
            var min = urlVault.GetToMinInt();
            string url = urlVault.GetBaseUrl() + urlVault.GetResolution() + "&from=" + from + "&to=" + to;

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

            int i = 0;

            while (LoadingPage)
            {
                i++;
                await Task.Delay(100);
                if (i == 1000)
                    break;
            }

            await GetAndSaveData();
        }

        private static async Task GetAndSaveData()
        {
            string html = "";
            HtmlDocument doc = new HtmlDocument();

            await _browser.GetSourceAsync().ContinueWith(taskHtml =>
            {
                html = taskHtml.Result;
            });
            doc = new HtmlDocument();
            doc.LoadHtml(html);
        }
    }
}
