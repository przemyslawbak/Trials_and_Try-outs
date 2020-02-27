using CefSharp.OffScreen;
using CefSharp.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.OffScreen
{
    public class Program
    {
        private static List<string> _urlList;
        private static List<string> _emailList;
        private static List<ChromiumWebBrowser> _browsersList;
        private static RequestContext _requestContext;

        static void Main(string[] args)
        {
            Task t = new Task(ExecuteProgram);
            t.Start();
            Console.ReadKey();

            Cef.Shutdown();
        }

        private static async void ExecuteProgram()
        {
            //create email list
            _emailList = CreateEmailList();
            //create url list
            _urlList = CreateUrlList();
            //create browsers list
            _browsersList = CreateBrowsersList();
            //create request context
            _requestContext = CreateContext();

            for (int i = 0; i < _browsersList.Count(); i++)
            {
                await TakeScreenAsync(_browsersList[i], i);
            }
        }

        private static async Task TakeScreenAsync(ChromiumWebBrowser browser, int i)
        {
            Thread.Sleep(2000);
            while (browser.IsLoading)
                await Task.Delay(50);
            var task = browser.ScreenshotAsync();
            await task.ContinueWith(x =>
            {
                // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
                var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CefSharp_screenshot" + i.ToString() + ".png");

                // Save the Bitmap to the path.
                // The image type is auto-detected via the ".png" extension.
                task.Result.Save(screenshotPath);

                // We no longer need the Bitmap.
                // Dispose it to avoid keeping the memory alive.  Especially important in 32-bit applications.
                task.Result.Dispose();

                Console.WriteLine("Screenshot saved.  Launching your default image viewer...");

                // Tell Windows to launch the saved image.
                Process.Start(new ProcessStartInfo(screenshotPath)
                {
                    // UseShellExecute is false by default on .NET Core.
                    UseShellExecute = true
                });

                Console.WriteLine("Image viewer launched.  Press any key to exit.");
            }, TaskScheduler.Default);
        }

        private static List<string> CreateEmailList()
        {
            List<string> list = new List<string>
            {
                "przemyslaw.pszemek@wp.pl",
                "7erikgantengzu@gamosbaptish.com"
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

            ChromiumWebBrowser browser2 = new ChromiumWebBrowser(_urlList[0], null, _requestContext, true);
            ChromiumWebBrowser browser1 = new ChromiumWebBrowser(_urlList[1], null, _requestContext, true);
            list.Add(browser1);
            list.Add(browser2);

            return list;
        }

        private static List<string> CreateUrlList()
        {
            List<string> list = new List<string>();

            const string dupaUrl = "https://www.meteoblue.com/pl/pogoda/tydzie%C5%84/wroc%c5%82aw_polska_3081368";
            const string testUrl = "https://languagetool.org/pl/";
            list.Add(dupaUrl);
            list.Add(testUrl);

            return list;
        }
    }
}
