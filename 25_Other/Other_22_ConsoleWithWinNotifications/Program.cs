using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            NotifyIcon trayIcon = new NotifyIcon();
            trayIcon.Text = "TestApp";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            ContextMenu trayMenu = new ContextMenu();

            trayMenu.MenuItems.Add("Blah", item1_Click);
            trayMenu.MenuItems.Add("Blah2", item1_Click);
            trayMenu.MenuItems.Add("Blah3", item1_Click);

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            Application.Run();
        }

        private static void item1_Click(object sender, EventArgs e)
        {
            //
        }
    }
}
