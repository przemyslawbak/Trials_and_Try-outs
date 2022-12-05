using CefSharp.OffScreen;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.OffScreen
{
    /// <summary>
    /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    /// Will not work because in CefSharp.OffScreen JS is disabled
    /// Possible fix:
    /// https://stackoverflow.com/a/53453064
    /// xxx - replace with correct strings
    /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    /// </summary>
    public class Program
    {
        private static int _min;
        private static int _max;
        private static int _delta;
        private static string _file = "_input.txt";
        private static string _output = "_output.txt";
        private static string _pass = "xxx";
        private static string _url = "xxx";
        private static List<string> _emailList;
        private static List<ChromiumWebBrowser> _browsersList;
        private static RequestContext _requestContext;
        private static EventHandler<LoadingStateChangedEventArgs> _pageLoadedEventHandler;

        static void Main(string[] args)
        {
            Task t = new Task(ExecuteProgram);
            t.Start();
            Console.ReadLine();

            Cef.Shutdown();
        }

        public static bool LoadingPage { get; set; } //if page is now loading

        private static async void ExecuteProgram()
        {
            //create email list
            _emailList = CreateEmailList();
            //create request context
            _requestContext = CreateContext();
            //create browsers list
            _browsersList = CreateBrowsersList();

            _min = await GetNumberFromFileAsync();
            _delta = GetDelta();
            _max = _min + _delta;

            for (int i = 0; i < _browsersList.Count(); i++)
            {
                await LoginAsync(_browsersList[i], i);
            }

            for (int i = 0; i < _browsersList.Count(); i++)
            {
                await Scrap(_browsersList[i]);
            }
        }

        private static async Task Scrap(IWebBrowser browser)
        {
            await SaveNumberToFileAsync();

            await browser.EvaluateScriptAsync(@"
			document.getElementById('xxx').value = '" + _min + @"';
			document.getElementById('xxx').value = '" + _max + @"';
			document.getElementById('buttonAdvSearch').click();
            ");

            await VerifyResults(browser);
        }

        private static async Task VerifyResults(IWebBrowser browser)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            LoadingPage = true;
            bool isResult;

            _pageLoadedEventHandler = async (sender, args) =>
            {
                if (args.IsLoading == false)
                {
                    browser.LoadingStateChanged -= _pageLoadedEventHandler;

                    try
                    {
                        isResult = await CheckForResultsAsync(browser);

                        if (isResult)
                        {
                            await GetAndSaveResults(browser);
                        }
                    }
                    catch (OperationCanceledException e)
                    {

                    }
                    catch (Exception e)
                    {

                    }
                    finally
                    {
                        LoadingPage = false;
                    }
                }
            };

            browser.LoadingStateChanged += _pageLoadedEventHandler; //activate

            while (LoadingPage)
                await Task.Delay(50); //if still loading

            stopWatch.Stop();
            await CheckTimer(stopWatch);

            bool pagination = await CheckForPagination(browser);

            if (pagination)
            {
                await LoadNextPage(browser);
                await VerifyResults(browser);
            }
            else
            {
                //bool banned = await VerifyIfBannedAsync(); //czy wprowadzać?
                _min = _max + 1;
                _max = _min + _delta;
                await Scrap(browser);
            }
        }

        private static async Task LoadNextPage(IWebBrowser browser)
        {
            await browser.EvaluateScriptAsync(@"
			const element = document.getElementsByClassName('pagination')[0];
            var listItem = element.getElementsByTagName('li');
            for (var i = 0; i < listItem.length; i++)
            {
                if (listItem[i].textContent.includes('>')){
                    listItem[i].children[0].click();
                }
            }
            ");
        }

        private static async Task<bool> CheckForPagination(IWebBrowser browser)
        {
            bool isPagination = false;

            Task<JavascriptResponse> verifyPagination = browser.EvaluateScriptAsync(@"
                (function()
                 {
                    const element = document.getElementsByClassName('pagination')[0];
                    var listItem = element.getElementsByTagName('li');
                    for (var i=0; i < listItem.length; i++) {
                        if (listItem[i].textContent.includes('>')){
                            return true;
                        }
                    }
                    return false;
                 })();
                ");

            await verifyPagination.ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    var response = t.Result;
                    var EvaluateJavaScriptResult = response.Success ? (response.Result ?? "null") : response.Message;
                    isPagination = Convert.ToBoolean(response.Result);
                }
            }
            );

            return isPagination;
        }

        private static async Task GetAndSaveResults(IWebBrowser browser)
        {
            List<string> imoList = new List<string>();

            HtmlDocument doc = await WaitForTable(browser);

            HtmlNode table = doc.DocumentNode.SelectSingleNode("//*[@id='xxx']/table");
            HtmlNodeCollection tableRows = table.SelectNodes("//tr");
            if (tableRows != null)
            {
                foreach (HtmlNode row in tableRows)
                {
                    if (row != null)
                    {
                        string imo = row.OuterHtml.ToString().Split('<')[3].Split('>')[1].Trim();
                        if (int.TryParse(imo, out int i))
                            imoList.Add(imo);
                    }
                }

                await SaveListToFileAsync(imoList);
            }
        }

        private static async Task SaveListToFileAsync(List<string> imoList)
        {
            foreach (string imo in imoList)
            {
                await SaveToFileAsync(imo + Environment.NewLine, _output);
            }
        }

        private static async Task<HtmlDocument> WaitForTable(IWebBrowser browser)
        {
            bool isReady = false;
            string html = "";
            HtmlDocument doc = new HtmlDocument();

            while (!isReady)
            {
                await Task.Delay(50);

                await browser.GetSourceAsync().ContinueWith(taskHtml =>
                {
                    html = taskHtml.Result;
                });

                doc = new HtmlDocument();
                doc.LoadHtml(html);

                HtmlNode table = doc.DocumentNode.SelectSingleNode("//*[@id='xxx']/table");
                HtmlNodeCollection tableRows = table.SelectNodes("//tr");

                if (tableRows.Count > 2)
                {
                    isReady = true;
                }
            }

            return doc;
        }

        private static async Task<bool> CheckForResultsAsync(IWebBrowser browser)
        {
            bool isResult = false;
            Task<JavascriptResponse> verifyTable = browser.EvaluateScriptAsync(@"
                (function()
                 {
                    const element = document.getElementById('xxx');
                    if (element)
                        { return true; }
                    else
                        { return false; }
                 })();
                ");

            await verifyTable.ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    var response = t.Result;
                    var EvaluateJavaScriptResult = response.Success ? (response.Result ?? "null") : response.Message;
                    isResult = Convert.ToBoolean(response.Result);
                }
            }
            );

            return isResult;
        }

        private static Task CheckTimer(Stopwatch stopWatch)
        {
            throw new NotImplementedException();
        }

        private static async Task SaveNumberToFileAsync()
        {
            if (File.Exists(_file))
            {
                File.Delete(_file);
            }

            await SaveToFileAsync(_min.ToString(), _file);
        }

        private static async Task SaveToFileAsync(string item, string path)
        {
            while (!IsFileReady(path))
            {
                await Task.Delay(5);
            }

            try
            {
                byte[] encodedText = Encoding.Unicode.GetBytes(item);

                using (FileStream sourceStream = new FileStream(path,
                    FileMode.Append, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true))
                {
                    await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                };
            }
            catch
            {
                await SaveToFileAsync(item, path);
            }

        }

        public static bool IsFileReady(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return inputStream.Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static int GetDelta()
        {
            int delta = 10;

            if (_min > 1000)
                delta = 20;
            if (_min > 2000)
                delta = 30;
            if (_min > 5000)
                delta = 50;

            return delta;
        }

        private static async Task<int> GetNumberFromFileAsync()
        {
            string grt = "";
            if (File.Exists(_file))
            {
                using (var reader = File.OpenText(_file))
                {
                    grt = await reader.ReadToEndAsync();
                }
            }
            else
            {
                return 100;
            }

            if (int.TryParse(grt, out int i))
            {
                return int.Parse(grt);
            }

            return 100;
        }

        private static async Task LoginAsync(ChromiumWebBrowser browser, int i)
        {
            await LoadPageAsync(browser, _url);

            await browser.EvaluateScriptAsync(@"document.getElementsByClassName('form-control')[0].value = '" + _emailList[i] + @"';");

            await browser.EvaluateScriptAsync(@"document.getElementsByClassName('form-control')[1].value = '" + _pass + @"';");

            await browser.EvaluateScriptAsync(@"document.getElementsByClassName('gris-bleu-copyright')[0].click();");

            await Task.Delay(1000);
            await browser.EvaluateScriptAsync(@"document.getElementById('advancedLink').click();");

            _pageLoadedEventHandler = (sender, args) =>
            {
                if (args.IsLoading == false)
                {
                    browser.LoadingStateChanged -= _pageLoadedEventHandler;

                    LoadingPage = false;
                }
            };

            browser.LoadingStateChanged += _pageLoadedEventHandler; //activate

            while (LoadingPage)
                await Task.Delay(50); //if still loading
        }

        private static Task LoadPageAsync(ChromiumWebBrowser browser, string address = null)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            EventHandler<LoadingStateChangedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                //Wait for while page to finish loading not just the first frame
                if (!args.IsLoading)
                {
                    browser.LoadingStateChanged -= handler;
                    tcs.TrySetResult(true);
                }
            };

            browser.LoadingStateChanged += handler;

            if (!string.IsNullOrEmpty(address))
            {
                browser.Load(address);
            }
            return tcs.Task;
        }

        private static List<string> CreateEmailList()
        {
            List<string> list = new List<string>
            {
                "xxx",
                "xxx"
            };

            return list;
        }

        private static RequestContext CreateContext()
        {
            var settings = new CefSettings()
            {
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            RequestContextSettings requestContextSettings = new RequestContextSettings();

            requestContextSettings.PersistSessionCookies = false;
            requestContextSettings.PersistUserPreferences = false;
            return new RequestContext(requestContextSettings);
        }

        private static List<ChromiumWebBrowser> CreateBrowsersList()
        {
            List<ChromiumWebBrowser> list = new List<ChromiumWebBrowser>();

            ChromiumWebBrowser browser2 = new ChromiumWebBrowser("", null, _requestContext, true);
            ChromiumWebBrowser browser1 = new ChromiumWebBrowser("", null, _requestContext, true);
            list.Add(browser1);
            list.Add(browser2);

            return list;
        }
    }
}
