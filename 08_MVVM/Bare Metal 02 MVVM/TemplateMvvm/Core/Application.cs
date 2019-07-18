using System;
using TemplateMvvm.Templates;
using TemplateMvvm.ViewModels;

namespace TemplateMvvm.Core
{
  public class Application
  {
    private static void Main()
    {
      Application application = new Application
      {
          DataContext = new PersonViewModel()
      };
      TemplateEngine.Instance.Add(new ApplicationTemplate());
      TemplateEngine.Instance.Add(new PersonTemplate());
      application.Run();

      Console.ReadKey();
    }

    public object DataContext { get; set; }

    public void Run()
    {
      TemplateBase template = TemplateEngine.Instance.FindTemplate(this);
      template.Render(DataContext);
    }
  }
}