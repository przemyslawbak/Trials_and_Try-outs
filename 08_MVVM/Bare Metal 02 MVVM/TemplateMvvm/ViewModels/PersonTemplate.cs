using System;
using TemplateMvvm.Templates;

namespace TemplateMvvm.ViewModels
{
  public class PersonTemplate : TemplateBase
  {
    public PersonTemplate()
    {
      TargetType = typeof (PersonViewModel);
    }

    public override void Render(dynamic dataContext)
    {
      Console.WriteLine(dataContext.Name);
      Console.ForegroundColor = ConsoleColor.DarkYellow;
      Console.WriteLine($"Was born on {dataContext.DateOfBirth,0:dddd, MMMM d, yyyy}");
    }
  }
}
