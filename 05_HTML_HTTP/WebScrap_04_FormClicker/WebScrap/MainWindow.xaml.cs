using CefSharp;
using CefSharp.Wpf;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;

namespace WebScrap
{
	public partial class MainWindow : Window
	{
		private ChromiumWebBrowser _wbPrzegladarka;
		private readonly InputSimulator _simulator;
		private EventHandler<LoadingStateChangedEventArgs> _pageLoadedEventHandler;
        private int _counter = 0;

		public bool LoadingPage { get; set; } //if page is now loading

		[DllImport("User32.dll")]
		private static extern bool SetCursorPos(int posX, int posY);

		public async Task MouseLeftButtonClickAsync(int posX, int posY)
		{
			await Task.Delay(1000);

			SetCursorPos(posX, posY);

			_simulator.Mouse.LeftButtonClick(); //click l.btn
		}

		public MainWindow()
		{
			_simulator = new InputSimulator();

			InitializeComponent();

			CreateBrowserAsync();
		}

		private async Task CreateBrowserAsync()
		{
			_wbPrzegladarka = new ChromiumWebBrowser();

			browserRow.Children.Add(_wbPrzegladarka);

            _wbPrzegladarka.Address = "https://docs.google.com/forms/d/e/1FAIpQLSfZ_qC9Dr8uAef64RMLRpdJJvRQQFfarZ6m_M4F8oW9Ns-gZg/viewform";

            await Crawl();
		}

		private async Task Crawl()
        {LoadingPage = true;

            _pageLoadedEventHandler = async (sender, args) =>
			{
				if (args.IsLoading == false)
				{
					_wbPrzegladarka.LoadingStateChanged -= _pageLoadedEventHandler;

					await _wbPrzegladarka.EvaluateScriptAsync(@"
			            window.scrollBy(0, 530);
			            ");


                    await _wbPrzegladarka.EvaluateScriptAsync(@"
			window.confirm = function() { return false; };
			");

                    await MouseLeftButtonClickAsync(700, 50);

					await MouseLeftButtonClickAsync(550, 910);

					LoadingPage = false;

                    await Task.Delay(3000);

                    LogAndAgain();
				}
			};

			_wbPrzegladarka.LoadingStateChanged += _pageLoadedEventHandler;

			while (LoadingPage)
				await Task.Delay(50); //if still crawling
		}

		private void LogAndAgain()
        {
            _counter++;

            File.AppendAllText("counter.txt", _counter.ToString() + Environment.NewLine);

            _wbPrzegladarka.Load("https://docs.google.com/forms/d/e/1FAIpQLSfZ_qC9Dr8uAef64RMLRpdJJvRQQFfarZ6m_M4F8oW9Ns-gZg/viewform");

            Crawl();
		}
	}
}
