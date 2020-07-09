using DinkToPdf;
using HtmlToPdf.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HtmlToPdf
{
    //https://github.com/rdvojmoc/DinkToPdf
    class Program
    {
        static void Main(string[] args)
        {
            GeneratePdfAsync();
        }

        private static void GeneratePdfAsync()
        {
            string name = "Przemysław Bąk";
            string company = "Google Inc.";
            string position = "Senior .Net developer";

            List<Project> projects = PopulateProjects();

            var converter = new BasicConverter(new PdfTools());
            HtmlToPdfDocument doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    UseCompression = true,
                    Margins = new MarginSettings(){ Top = 2, Left = 0, Right = 0, Bottom = 0 }
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = CreateHtml(name, company, position, projects),
                        WebSettings = { DefaultEncoding = "utf-8", EnableJavascript = true },
                        FooterSettings = { FontSize = 9, Right = "Przemyslaw Bak - Page [page] of [toPage]", Line = true}
                    }
                }
            };

            byte[] pdf = converter.Convert(doc);

            ByteArrayToFile("dupa.pdf", pdf);
        }

        private static List<Project> PopulateProjects()
        {
            return new List<Project>()
            {
                new Project()
                {
                    CompletionDate =new DateTime(2017, 3, 11),
                    Name = "Some very long project name",
                    Comments = "Desktop WPF application created for private and commercial usage. The solution is using popular search engine to get specified data, bypassing captcha verifications.",
                    GitHubUrl = "https://github.com/przemyslawbak/someotherproject",
                    WebUrl = "https://4sea-data.com/"
                },
                new Project()
                {
                    CompletionDate =new DateTime(2019, 1, 15),
                    Name = "Some other project name",
                    Comments = "Simple in use library for logging called meber methods, exceptions, properties with parameters and their values, and saving them in clear and transparent format.",
                    GitHubUrl = "https://github.com/przemyslawbak/someproj",
                    WebUrl = "https://google.com/"
                },
                new Project()
                {
                    CompletionDate =new DateTime(2018, 1, 19),
                    Name = "Some new project",
                    Comments = "This is a DEMO solution presenting Web API project processing raw POST and GET requests, and console application consuming the Web API, automatically creating database and sending requests.",
                    GitHubUrl = "https://github.com/przemyslawbak/someproj",
                    WebUrl = "#"
                },
                new Project()
                {
                    CompletionDate =new DateTime(2016, 2, 18),
                    Name = "Some new project",
                    Comments = "General purpose of the solution is to compute potentially best horse in a horse racing competition based on historic data available in subject related web services, and compare it with other horses starting in the race. The application is created for personal use and for educational purposes to practice different frameworks and approaches, and should not be use by ANYONE for making any betting decisions based on this application output. Author is taking no responsibility if someone loses any money when making decisions on the output of this solution.",
                    GitHubUrl = "https://github.com/przemyslawbak/someproj",
                    WebUrl = "#"
                },
                new Project()
                {
                    CompletionDate =new DateTime(2017, 3, 11),
                    Name = "Some new project",
                    Comments = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                    GitHubUrl = "https://github.com/przemyslawbak/someotherproject",
                    WebUrl = "https://4sea-data.com/"
                },
                new Project()
                {
                    CompletionDate =new DateTime(2019, 1, 15),
                    Name = "Some other project",
                    Comments = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                    GitHubUrl = "https://github.com/przemyslawbak/someproj",
                    WebUrl = "https://google.com/"
                },
                new Project()
                {
                    CompletionDate =new DateTime(2018, 1, 19),
                    Name = "Some new project",
                    Comments = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                    GitHubUrl = "https://github.com/przemyslawbak/someproj",
                    WebUrl = "#"
                },
                new Project()
                {
                    CompletionDate =new DateTime(2016, 2, 18),
                    Name = "Some new project",
                    Comments = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                    GitHubUrl = "https://github.com/przemyslawbak/someproj",
                    WebUrl = "#"
                }
            };
        }

        public static void ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
            }
        }

        private static string CreateHtml(string name, string company, string position, List<Project> projects)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<!DOCTYPE html><html>");
            sb.Append(CreateHtmlHead());
            sb.Append(@"<body>");
            sb.Append(CreateCvHeader(name, company, position));
            sb.Append(CreateCvProjects(projects));
            sb.Append(CreateCvWorkHistory());
            sb.Append(CreateCvEducation());
            sb.Append(CreateCvSkills());
            sb.Append(CreateCvInterests());
            sb.Append(CreateHtmlFooter());
            sb.Append(@"</body>");
            sb.Append(@"</html>");

            return sb.ToString();
        }

        private static string CreateCvInterests()
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }

        private static string CreateCvSkills()
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }

        private static string CreateCvWorkHistory()
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }

        private static string CreateCvEducation()
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }

        private static string CreateCvProjects(List<Project> projects)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class='sectionTitle'>Recent projects:</div>");
            foreach (var project in projects)
            {
                sb.Append(createSingleProjectDisplay(project));
            }

            return sb.ToString();
        }

        private static string createSingleProjectDisplay(Project project)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
<div class='projectContainer'>
    <div class='projectName'>" + project.Name + @"</div>
    <div class='projectComments'><b>About:</b> " + project.Comments + @"</div>
    <div class='projectTech'><b>Tech:</b> VS2017, C#, JS, HTML, WPF, MVVM, LINQ, Git, Prism, Autofac, xUnit, CefSharp, Html Agility Pack, NLog</div>
</div>
");

            return sb.ToString();
        }

        private static string CreateCvHeader(string name, string company, string position)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
<div class='aboutMainContainer'>
    <img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_06_CV_Spike\HtmlToPdf\bin\Debug\netcoreapp2.2\pic.jpg'>
    <div class='personalData'>
        <div class='name'>" + name + @"</div>
        <div>" + position + @"</div>
        <div>in " + company + @"</div>
    </div>
    <div class='sideInfo'>
        <div class='sideText'>
            <div>+48 725 875 135</div>
            <div>przemyslaw.bak@simple-mail.net</div>
            <div>przemyslaw-bak.pl</div>
            <div>github.com/przemyslawbak</div>
            <div>Wrocław</div>
        </div>
        <div class='sideImages'>
            <img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_06_CV_Spike\HtmlToPdf\bin\Debug\netcoreapp2.2\phone.png' />
            <img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_06_CV_Spike\HtmlToPdf\bin\Debug\netcoreapp2.2\email.png' />
            <img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_06_CV_Spike\HtmlToPdf\bin\Debug\netcoreapp2.2\world.png' />
            <img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_06_CV_Spike\HtmlToPdf\bin\Debug\netcoreapp2.2\github.png' />
            <img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_06_CV_Spike\HtmlToPdf\bin\Debug\netcoreapp2.2\house.png' />
        </div>
    </div>
</div>
");

            return sb.ToString();
        }

        private static string CreateHtmlFooter()
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }

        private static string CreateHtmlHead()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<head>");
            sb.Append(@"<link href='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_06_CV_Spike\HtmlToPdf\bin\Debug\netcoreapp2.2\styles.css' rel='stylesheet' type='text/css' media='screen'/>");
            sb.Append(@"</head>");

            return sb.ToString();
        }
    }
}
