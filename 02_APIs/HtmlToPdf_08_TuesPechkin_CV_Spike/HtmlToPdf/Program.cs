using HtmlToPdf.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using TuesPechkin;

namespace HtmlToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            GeneratePdf();
        }

        private static List<Technology> PopulateTechs()
        {
            return new List<Technology>()
            {
                new Technology()
                {
                    Name = "Ajax/Fetch",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/ajax.png?v=UYu5Lq6n4NCNbOoNLf4VeWvnsdmX2RXoUKzKEPjUXLI"
                },
                new Technology()
                {
                    Name = "Android",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/android.png?v=ossNsrBYjzKx9tFgvarP9B8p9Q5IkqVQfACshi-pObw"
                },
                new Technology()
                {
                    Name = "ASP.NET Core",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/netcore.png?v=Dm0FjSKve5XRiR97ZlYWkpZNYrylGx0_LcHh4l4UpJM"
                },
                new Technology()
                {
                    Name = "Bootstrap",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/bootstrap.png?v=h7EsRHTUu5zCy2ksQrV9kqAsX2hVu4iqWS1yf81kDsg"
                },
                new Technology()
                {
                    Name = "jQuery",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/jquery.png?v=GEy_t7EqgB3kiM6zxgU07kS34BRXYqNfjJDe3MpMNmA"
                },
                new Technology()
                {
                    Name = "MVVM",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/pattern.png?v=kL49Wcigqn9T3OoO8-v3lOAx_9Dk672-Jc1g-yIQwkM"
                },
                new Technology()
                {
                    Name = "NLog",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/nlog.png?v=YgKi9phEhQCQOrNHsVsl_hYlemaZ5-ame79svp93Tp4"
                },
                new Technology()
                {
                    Name = "Razor Views",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/dotnet.png?v=uEvFPZAVyAzeJpSjCg7A2IIxWtPalZm1di3yPvM6lRA"
                },
                new Technology()
                {
                    Name = "SQLite",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/sqlite.png?v=tVbEKxwMBhr1iSjRsb6dI-qOZq0lmF67ub0vz7D6EJE"
                },
                new Technology()
                {
                    Name = "WPF",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/microsoft.png?v=E1G1_6lkh8bGOUfTH0CMSwjKKCMVnH64ivcrIBRKSAg"
                },
                new Technology()
                {
                    Name = "Xamarin",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/xamarin.png?v=-YqnDDPrNcFRZhD5QROV0lFVQCe056JEYe_J03N3L7o"
                },
                new Technology()
                {
                    Name = "XML",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/xml.png?v=-1vB5mV-olfBKLkUhPOhKtTmcCeXR_FuaXfEuwqNPGA"
                },
                new Technology()
                {
                    Name = "WPF",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/microsoft.png?v=E1G1_6lkh8bGOUfTH0CMSwjKKCMVnH64ivcrIBRKSAg"
                },
                new Technology()
                {
                    Name = "Xamarin",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/xamarin.png?v=-YqnDDPrNcFRZhD5QROV0lFVQCe056JEYe_J03N3L7o"
                },
                new Technology()
                {
                    Name = "XML",
                    PictureLink = "http://przemyslaw-bak.pl/src/img/technologies/xml.png?v=-1vB5mV-olfBKLkUhPOhKtTmcCeXR_FuaXfEuwqNPGA"
                },
            };
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
                    WebUrl = "#"
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
                    GitHubUrl = "#",
                    WebUrl = "https://4sea-data.com/"
                },
                new Project()
                {
                    CompletionDate =new DateTime(2019, 1, 15),
                    Name = "Some other project",
                    Comments = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                    GitHubUrl = "https://github.com/przemyslawbak/someproj",
                    WebUrl = "https://google.com/"
                }
            };
        }

        private static void GeneratePdf()
        {
            string name = "Przemyslaw Bak";
            string company = "Google Inc.";
            string position = "Senior .Net developer";

            List<Project> projects = PopulateProjects();

            List<Technology> technologies = PopulateTechs();

            IConverter converter =
    new ThreadSafeConverter(
        new PdfToolset(
            new Win32EmbeddedDeployment(
                new TempFolderDeployment())));

            HtmlToPdfDocument doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ProduceOutline = true,
                    DocumentTitle = "Pretty stuff",
                    PaperSize = PaperKind.A4, // Implicit conversion to PechkinPaperSize
                    ImageQuality = 100,
                UseCompression = true,
                ColorMode = GlobalSettings.DocumentColorMode.Color,
                    Margins =
                    {
                        Right = 1,
                        Left = 1,
                        Top = 1
                    }
                },
                Objects = {
                    new ObjectSettings
                    {
                        HtmlText = GetHtml(name, company, position, projects, technologies),
                        WebSettings = { UserStyleSheet = @"file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\styles.css" },
                        CountPages = true,
                        FooterSettings = { FontSize = 9, LeftText = "Genereted by: github.com/przemyslawbak/CV_Creator", RightText = "Przemyslaw Bak - Page [page] of [toPage]", UseLineSeparator = true }
                    }
                }
            };

            byte[] pdf = converter.Convert(doc);

            ByteArrayToFile("dupa.pdf", pdf);
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

        private static string GetHtml(string name, string company, string position, List<Project> projects, List<Technology> technologies)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<!DOCTYPE html><html>");
            sb.Append(@"<body>");
            sb.Append(CreateCvHeader(name, company, position));
            sb.Append(CreateCvProjects(projects));
            //sb.Append(CreateTechStack(technologies));
            //sb.Append(CreateCvEmploymentHistory());
            //sb.Append(CreateCvEducation());
            //sb.Append(CreateCvInterests());
            //sb.Append(CreateHtmlFooter(company));
            //sb.Append(@"</body>");
            //sb.Append(@"</html>");

            return sb.ToString();
        }

        private static string CreateTechStack(List<Technology> technologies)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class='sectionTitle'>Technology stack:</div>");
            sb.Append(@"<div class='techContainer'>");
            foreach (var technology in technologies)
            {
                sb.Append(CreateSingleTechDisplay(technology));
            }
            sb.Append(@"</div>");

            return sb.ToString();
        }

        private static string CreateSingleTechDisplay(Technology technology)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
<div class='singleTech'>
    <img class='techPic' src='" + technology.PictureLink + @"'>
    <div class='techName'>" + technology.Name + @"</div>
</div>
");

            return sb.ToString();
        }

        private static string CreateCvInterests()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class='sectionTitle'>Hobbies and interests:</div>");
            sb.Append(@"
<div class='interestsContainer'>
    <div class='interestsImages'>
        <img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\financial.png' />
        <img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\sport.png' />
        <img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\book.png' />
    </div>
    <div class='interestsText'>
        <div>I have been interested in financial markets for many years, I observe the situation on global markets.</div>
        <div>For many years I am training calisthenics, as time allows, with breaks matrial arts, and running at a medium distances.</div>
        <div>I am interested in social psychology, I listen to SWPS lectures, if time permits, I sometimes read publications.</div>
    </div>
</div>
");
            return sb.ToString();
        }

        private static string CreateCvEmploymentHistory()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class='sectionTitle'>Work history:</div>");
            sb.Append(@"
<div class='historyContainer'>
    <div class='workHistory'>
        <div class='workTitle'>C# WPF, ASP.NET developer</div>
        <div class='workPeriod'>
            04.2017 - present
        </div>
        <div class='workDescription'>
            I am self-employed, working on projects for my private and commercial use. I am responsible for front-end and back-end. Creating from scratch ASP.NET Core web applications, WPF desktop applications, writing unit tests for my projects, using good practices of programming, based on SOLID principles, while learning from the best in the internet. Working with async programming, identity, desingn and achitectural patterns, unit testing, various APIs, graphic editors, planning all from scratch.
        </div>
    </div>
    <div class='workHistory'>
        <div class='workTitle'>Offshore industry freelancer</div>
        <div class='workPeriod'>
            06.2011 - present
        </div>
        <div class='workDescription'>
            Work in the offshore oil-gas and renewable energy industries in operational and management positions, as a team member participating in projects of various levels of complexity.
        </div>
    </div>
    <div class='workHistory'>
        <div class='workTitle'>Maritime transport freelancer</div>
        <div class='workPeriod'>
            08.2005 - 05.2011
        </div>
        <div class='workDescription'>
            Work as a staff member and operational positions at maritime cargo transportation industry.
        </div>
    </div>
</div>
");
            return sb.ToString();
        }

        private static string CreateCvEducation()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class='sectionTitle'>Education:</div>");
            sb.Append(@"
<div class='historyContainer'>
    <div class='workHistory'>
        <div class='workTitle'>Gdynia Maritime University</div>
        <div class='workPeriod'>
            10.2003 - 06.2007
        </div>
        <div class='workDescription'>
            Bachelor of Engineering in Navigation, specialization: Sea Transport.
        </div>
    </div>
</div>
");

            return sb.ToString();
        }

        private static string CreateCvProjects(List<Project> projects)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class='sectionTitle'>Recent projects:</div>");
            foreach (var project in projects)
            {
                sb.Append(CreateSingleProjectDisplay(project));
            }

            return sb.ToString();
        }

        private static string CreateSingleProjectDisplay(Project project)
        {
            if (project.Comments.Length > 175)
            {
                int spaceIndex = project.Comments.IndexOf(" ", 175);
                try
                {
                    project.Comments = project.Comments.Substring(0, spaceIndex) + " (...)";
                }
                catch
                {
                    project.Comments = project.Comments;
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
<div class='projectContainer'>
	<div class='projectName'>" + project.Name + @"</div>
	<div class='projectComments'><b>About:</b> " + project.Comments + @"</div>
	<div class='projectTech'><b>Tech:</b> VS2017, C#, JS, HTML, WPF, MVVM, LINQ, Git, Prism, Autofac, xUnit, CefSharp, Html Agility Pack, NLog</div>
	<div class='projImages'>
		<img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\info_brown.png' />");
            if (project.GitHubUrl != "#")
            {
                sb.Append(@"<img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\github_brown.png' />");
            };
            if (project.WebUrl != "#")
            {
                sb.Append(@"<img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\world_brown.png' />");
            };
            sb.Append(@"
	</div>
	<div class='projText'>
		<div><a class='projectLink' href='http://przemyslaw-bak.pl/myprojects/details?projectid=" + project.ProjectID + @"'>more about on: przemyslaw-bak.pl</a></div>");
            if (project.GitHubUrl != "#")
            {
                sb.Append(@"<div><a class='projectLink' href=" + project.GitHubUrl + @" '>" + project.GitHubUrl.Replace("https://", "") + @"</a></div>");
            };
            if (project.WebUrl != "#")
            {
                sb.Append(@"<div><a class='projectLink' href='" + project.WebUrl + @"'>" + project.WebUrl.Replace("https://", "") + @"</a></div>");
            };
            sb.Append(@"
	</div>
</div>
");
            return sb.ToString();
        }

        private static string CreateCvHeader(string name, string company, string position)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
<div class='aboutMainContainer'>
	<img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\pic.jpg'>
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
			<div>Wroclaw</div>
		</div>
		<div class='sideImages'>
			<img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\phone.png' />
			<img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\email.png' />
			<img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\world.png' />
			<img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\github.png' />
			<img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_08_TuesPechkin_CV_Spike\HtmlToPdf\bin\Debug\house.png' />
		</div>
	</div>
</div>
");

            return sb.ToString();
        }

        private static string CreateHtmlFooter(string company)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
<div class='footerContainer'>
    <div class='footerText'>
        I agree to the processing of personal data provided in this document for realising the recruitment process pursuant to the Personal Data Protection Act of 10 May 2018 (Journal of Laws 2018, item 1000) and in agreement with Regulation (EU) 2016/679 of the European Parliament and of the Council of 27 April 2016 on the protection of natural persons with regard to the processing of personal data and on the free movement of such data, and repealing Directive 95/46/EC (General Data Protection Regulation) I hereby consent " + company + @" to administrate, process and store my personal data for the purpose of recruitment processes including sharing my details with potential future employer for whom " + company + @" performs work to establish conditions of engagement before concluding a contract of employment. The agreement covers the processing of personal data by " + company + @" even after recruitment process in order to present further proposals of employment and may be revoked at any time. I declare that all the data that are included in the CV and job applications have been delivered to the company " + company + @" voluntarily and they are true.
    </div>
</div>
");

            return sb.ToString();
        }
    }
}
