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

            List<Technology> technologies = PopulateTechs();

			var converter = new BasicConverter(new PdfTools());
			HtmlToPdfDocument doc = new HtmlToPdfDocument()
			{
				GlobalSettings = {
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					UseCompression = true,
					Margins = new MarginSettings(){ Top = 2, Left = 0, Right = 0}
				},
				Objects = {
					new ObjectSettings() {
						HtmlContent = CreateHtml(name, company, position, projects, technologies),
                        PagesCount = true,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        FooterSettings = { FontSize = 9, Left = "Genereted by: github.com/przemyslawbak/CV_Creator", Right = "Przemyslaw Bak - Page [page] of [toPage]", Line = true}
                    }
				}
			};

			byte[] pdf = converter.Convert(doc);

			ByteArrayToFile("dupa.pdf", pdf);
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

		private static string CreateHtml(string name, string company, string position, List<Project> projects, List<Technology> technologies)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(@"<!DOCTYPE html><html>");
			sb.Append(CreateHtmlHead());
			sb.Append(@"<body>");
			sb.Append(CreateCvHeader(name, company, position));
			sb.Append(CreateCvProjects(projects));
			sb.Append(CreateTechStack(technologies));
			sb.Append(CreateCvEmploymentHistory());
			sb.Append(CreateCvEducation());
			sb.Append(CreateCvSkills());
			sb.Append(CreateCvInterests());
			sb.Append(CreateHtmlFooter());
			sb.Append(@"</body>");
			sb.Append(@"</html>");

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

			return sb.ToString();
		}

		private static string CreateCvSkills()
		{
			StringBuilder sb = new StringBuilder();

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
            Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of de Finibus Bonorum et Malorum (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, Lorem ipsum dolor sit amet..., comes from a line in section 1.10.32.
        </div>
    </div>
    <div class='workHistory'>
        <div class='workTitle'>Offshore industry freelancer</div>
        <div class='workPeriod'>
            06.2011 - present
        </div>
        <div class='workDescription'>
            It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).
        </div>
    </div>
</div>
");
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
				sb.Append(CreateSingleProjectDisplay(project));
			}

			return sb.ToString();
		}

		private static string CreateSingleProjectDisplay(Project project)
		{
            if (project.Comments.Length > 170)
            {
                int spaceIndex = project.Comments.IndexOf(" ", 170);
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
		<img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_06_CV_Spike\HtmlToPdf\bin\Debug\netcoreapp2.2\info_brown.png' />");
			if (project.GitHubUrl != "#")
			{
				sb.Append(@"<img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_06_CV_Spike\HtmlToPdf\bin\Debug\netcoreapp2.2\github_brown.png' />");
			};
			if (project.WebUrl != "#")
			{
				sb.Append(@"<img src='file:///C:\Users\asus\Desktop\IT\!!Trials!!\02_APIs\HtmlToPdf_06_CV_Spike\HtmlToPdf\bin\Debug\netcoreapp2.2\world_brown.png' />");
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
