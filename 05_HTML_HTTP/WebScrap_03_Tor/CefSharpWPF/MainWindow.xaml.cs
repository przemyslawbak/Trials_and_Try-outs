using CefSharp;
using CefSharp.Wpf;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Tor;
using Tor.Config;

namespace CefSharpWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Client client;
        private void InitializeTor()
        {
            Process[] previous = Process.GetProcessesByName("tor");
            //tutaj zabij proces
            ClientCreateParams createParams = new ClientCreateParams();
            createParams.ConfigurationFile = "";
            createParams.DefaultConfigurationFile = "";
            createParams.ControlPassword = "";
            createParams.ControlPort = 9051;
            createParams.Path = @"Tor\Tor\tor.exe";
            createParams.SetConfig(ConfigurationNames.AvoidDiskWrites, true);
            createParams.SetConfig(ConfigurationNames.GeoIPFile, System.IO.Path.Combine(Environment.CurrentDirectory, @"Tor\Data\Tor\geoip"));
            createParams.SetConfig(ConfigurationNames.GeoIPv6File, System.IO.Path.Combine(Environment.CurrentDirectory, @"Tor\Data\Tor\geoip6"));

            client = Client.Create(createParams);

        }

        private void ConfigureBrowser()
        {
            CefSettings settings = new CefSettings();
            settings.CefCommandLineArgs.Add("proxy-server", "socks5proxy");
            Cef.Initialize(settings);
        }
        public MainWindow()
        {
            InitializeTor();
            ConfigureBrowser();
            InitializeComponent();
            wbPrzegladarka.Address = "https://www.iplocation.net/find-ip-address";
            wbPrzegladarka.FrameLoadEnd += ChromiumBrowser_OnFrameLoadEndAsync;
        }
        private async void ChromiumBrowser_OnFrameLoadEndAsync(object sender, FrameLoadEndEventArgs e)
        {
            wbPrzegladarka.FrameLoadEnd -= ChromiumBrowser_OnFrameLoadEndAsync;
            if (!client.IsRunning)
            {
                await Task.Delay(5000);
            }
            else
            {
                await Task.Delay(10000);
                RouterCollection routers = client.Status.GetAllRouters();
                Kontynuacja();
            }
        }
        private void Kontynuacja()
        {
            client.Controller.CleanCircuits();
            client.Controller.CreateCircuit();
            wbPrzegladarka.Load("https://www.iplocation.net/find-ip-address");
            wbPrzegladarka.FrameLoadEnd += ChromiumBrowser_OnFrameLoadEndAsync;
        }

    }
}
