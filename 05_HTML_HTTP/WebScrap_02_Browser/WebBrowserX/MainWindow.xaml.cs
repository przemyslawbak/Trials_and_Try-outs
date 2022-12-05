using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebBrowserX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // Metody obsługi zdarzeń kliknięcia opcji Menu
        private void RamkaOn_Click(object sender, RoutedEventArgs e) // Włączenie ramki
        {
            if (brdRamka != null)
                brdRamka.BorderThickness = new Thickness(3);
        }
        private void RamkaOff_Click(object sender, RoutedEventArgs e) // Wyłączenie ramki
        {
            if (brdRamka != null)
                brdRamka.BorderThickness = new Thickness(0);
        }
        private void Zapisz_Click(object sender, RoutedEventArgs e) // Zapisanie strony
                                                                    // do pliku
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "WebPage|*.html";
            dialog.DefaultExt = ".html";
            dynamic doc = wbPrzegladarka.Document;
            if (doc != null)
            {
                var htmlText = doc.documentElement.InnerHtml;
                if (dialog.ShowDialog() == true && htmlText != null)
                {
                    File.WriteAllText(dialog.FileName, htmlText); // File wymaga
                                                                  // using System.IO;
                }
            }
        }
        private void Tmp_Click(object sender, RoutedEventArgs e) // Tymczasowa metoda
                                                                 // dla niegotowych opcji
        {
            MessageBox.Show("Opcja w budowie");
        }
        private void OProgramie_Click(object sender, RoutedEventArgs e) // Informacje
                                                                        // o programie
        {
            MessageBox.Show("Prosta przeglądarka www, Wersja 1.0, Helion 2017");
        }
        private void Exit_Click(object sender, RoutedEventArgs e) // Wyjście (zamknięcie
                                                                  // okna aplikacji)
        {
            Close();
        }
        // Metody obsługi zdarzeń dla kontrolek umieszczonych w ToolBar
        private void btnWejdz_Click(object sender, RoutedEventArgs e)
        {
            wbPrzegladarka.Navigate(txtAdres.Text);
        }
        private void btnWstecz_Click(object sender, RoutedEventArgs e)
        {
            if (wbPrzegladarka.CanGoBack)
                wbPrzegladarka.GoBack();
        }
        private void btnDalej_Click(object sender, RoutedEventArgs e)
        {
            if (wbPrzegladarka.CanGoForward)
                wbPrzegladarka.GoForward();
        }
        private void txtAdres_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                wbPrzegladarka.Navigate(txtAdres.Text);
        }
        // Metody obsługi zdarzeń dla kontrolki WebBrowser (Navigating i Navigated)
        private void wbPrzegladarka_Navigating(object sender,
        System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            txtAdres.Text = e.Uri.OriginalString; // Aktualizacja pola tekstowego z adresem
        }
        private void wbPrzegladarka_Navigated(object sender, NavigationEventArgs e)
        {
            HideScriptErrors(wbPrzegladarka, true); // Wywołanie metody ukrywającej błędy
                                                    // JavaScriptu
        }
        public void HideScriptErrors(WebBrowser wb, bool Hide)
        {
            // Ukrycie błędów JavaScriptu, rozwiązanie ze strony MSDN "Suppress Script Errors in
            // Windows.Controls.Webbrowser"
            // Typ wyliczeniowy BindingFlags wymaga przestrzeni nazw using System.Reflection;
            dynamic activeX = this.wbPrzegladarka.GetType().InvokeMember("ActiveXInstance",
            BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, this.wbPrzegladarka, new object[] { });
            activeX.Silent = true;
        }
    }
}
