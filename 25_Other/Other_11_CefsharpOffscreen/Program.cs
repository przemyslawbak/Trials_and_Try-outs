using CefSharp.OffScreen;
using HtmlAgilityPack;
using System;
using System.Drawing;
using System.IO;
using System.Security.Policy;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.OffScreen
{
    public class Program
    {
        private static string _url = "https://www.cboe.com/us/options/";
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
            _requestContext = await CreateContextAsync();

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

        private static async Task<RequestContext> CreateContextAsync()
        {
            using (CefSettings settings = new CefSettings())
            {
                settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
                settings.LogSeverity = LogSeverity.Disable; //disabled console logs
                string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "temp_files");
                settings.CachePath = Path.Combine(basePath, "CefSharp\\Cache");
                string path = Path.Combine(basePath, "Cookies");
                settings.RemoteDebuggingPort = 8080;
                settings.CachePath = path;

                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null); //init

                var mng = Cef.GetGlobalCookieManager();
                Cookie privacyCookie = new Cookie();
                privacyCookie.Name = "privacy";
                privacyCookie.Value = "1680967824";
                privacyCookie.Domain = "stooq.com";
                privacyCookie.Path = "/";
                privacyCookie.Expires = DateTime.Now.AddDays(7);
                Cookie cookieUser = new Cookie();
                cookieUser.Name = "cookie_user";
                cookieUser.Value = "%3F0001dllg000012400d1300e3%7Cwig20";
                cookieUser.Domain = ".stooq.com";
                cookieUser.Path = "/";
                cookieUser.Expires = DateTime.Now.AddDays(7);
                Cookie uidCookie = new Cookie();
                uidCookie.Name = "uid";
                uidCookie.Value = "plddz23if7pordp5xd29162tko";
                uidCookie.Domain = "stooq.com";
                uidCookie.Path = "/";
                uidCookie.Expires = DateTime.Now.AddDays(7);
                Cookie uuCookie = new Cookie();
                uuCookie.Name = "cookie_uu";
                uuCookie.Value = "230408000";
                uuCookie.Domain = ".stooq.com";
                uuCookie.Path = "/";
                uuCookie.Expires = DateTime.Now.AddDays(7);
                Cookie PHPSESSIDCookie = new Cookie();
                PHPSESSIDCookie.Name = "PHPSESSID";
                PHPSESSIDCookie.Value = "7atcqomu8glaqm1v6do17o30h6";
                PHPSESSIDCookie.Domain = "stooq.com";
                PHPSESSIDCookie.Path = "/";
                PHPSESSIDCookie.Expires = DateTime.Now.AddDays(7);
                Cookie FCNECCookie = new Cookie();
                FCNECCookie.Name = "FCNEC";
                FCNECCookie.Value = "%5B%5B%22AKsRol_TQDMtfszLfiR5UWk2L5EV9HQoPA3moopIg9pojY4X8jXiN_njPZ1NmnIwjhyutsnxNa8pf7Z764-CxBLfrxfoV2_1j-AjgOP9nKsQFu5-zE6fpRFo4T2A1sjJKbFundG9IZ5lRdIH5jA4YlQOZUODEx24eA%3D%3D%22%5D%2Cnull%2C%5B%5D%5D";
                FCNECCookie.Domain = ".stooq.com";
                FCNECCookie.Path = "/";
                FCNECCookie.Expires = DateTime.Now.AddDays(7);
                Cookie FCCDCFCookie = new Cookie();
                FCCDCFCookie.Name = "FCCDCF";
                FCCDCFCookie.Value = "%5Bnull%2Cnull%2Cnull%2C%5B%22CPp5tEAPp5tEAEsABBPLC_CoAP_AAG_AAB5YINJB7D7FbSFCyP57aLsAMAhXRkCAQqQCAASBAmABQAKQIAQCkkAYFESgBAACAAAgICJBIQIMCAgACUABQAAAAAEEAAAABAAIIAAAgAEAAAAIAAACAIAAEAAIAAAAEAAAmQhAAIIACAAAhAAAIAAAAAAAAAAAAgCAAAAAAAAAAAAAAAAAAQQaQD2F2K2kKEkfjWUWYAQBCujIEAhUAEAAECBIAAAAUgQAgFIIAwAIlACAAAAABAQEQCQgAQABAAAoACgAAAAAAAAAAAAAAQQAABAAIAAAAAAAAEAQAAIAAQAAAAAAABEhCAAQQAEAAAAAAAQAAAAAAAAAAABAAAAAAAAAAAAAAAAAAAAgAA.f-gAAAAAAAA%22%2C%221~2072.70.89.93.108.122.149.196.2253.2299.259.2357.311.317.323.2373.338.358.2415.415.2506.2526.482.486.494.495.2568.2571.2575.540.574.2624.624.2677.827.864.981.1048.1051.1095.1097.1171.1201.1205.1276.1301.1365.1415.1449.1570.1577.1651.1716.1753.1765.1870.1878.1889.1958%22%2C%22D9648DBA-77EF-4900-AD90-61E97FF8AA2F%22%5D%2Cnull%2Cnull%2C%5B%5D%5D";
                FCCDCFCookie.Domain = ".stooq.com";
                FCCDCFCookie.Path = "/";
                FCCDCFCookie.Expires = DateTime.Now.AddDays(7);

                await mng.SetCookieAsync("https://stooq.com/", FCCDCFCookie);
                await mng.SetCookieAsync("https://stooq.com/", PHPSESSIDCookie);
                await mng.SetCookieAsync("https://stooq.com/", FCNECCookie);
                await mng.SetCookieAsync("https://stooq.com/", uuCookie);
                await mng.SetCookieAsync("https://stooq.com/", uidCookie);
                await mng.SetCookieAsync("https://stooq.com/", privacyCookie);
                await mng.SetCookieAsync("https://stooq.com/", cookieUser);
            }

            RequestContextSettings requestContextSettings = new RequestContextSettings();

            requestContextSettings.PersistSessionCookies = false; //separated cookies for each instance
            requestContextSettings.PersistUserPreferences = false; //separated user cred for each instance

            return new RequestContext(requestContextSettings);
        }

        private static async Task RunProgramAsync()
        {
            await LoadPageWithPrivacy(_url);
        }

        private static async Task LoadPageWithPrivacy(string url)
        {
            _browser.Size = new Size(2000, 2500);

            LoadingPage = true;

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

            await Task.Delay(1000);

            var html = GetHtml();
        }

        private static async Task GetHtml()
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
