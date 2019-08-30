using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf_.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            string workingDirectory = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString();
            string name = Assembly.GetEntryAssembly().GetName().Name;
            string path = Path.Combine(workingDirectory, "log.config");
            if (File.Exists(path))
            {
                //MessageBox.Show("Assembly works!");
            }

            bool ok = false;

            string newDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string[] files = Directory.GetFiles(newDirectory, "log.config", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                List<string> lines = ReadFileLines(file);

                if (lines.Any(l => l.Contains("logFile")) && lines.Any(l => l.Contains("debugOnly")))
                    ok = true;
            }

            if (ok)
            {
                MessageBox.Show("Searching works!");
            }
        }

        private List<string> ReadFileLines(string file)
        {
            using (var reader = File.OpenText(file))
            {
                var fileText = reader.ReadToEnd();

                var array = fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                return new List<string>(array);
            }
        }
    }
}
