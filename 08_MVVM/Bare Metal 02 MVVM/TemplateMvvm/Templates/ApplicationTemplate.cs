using System;
using TemplateMvvm.Core;

namespace TemplateMvvm.Templates
{
  public class ApplicationTemplate : TemplateBase
  {
    public ApplicationTemplate()
    {
      TargetType = typeof (Application);
    }

    public override void Render(dynamic dataContext)
    {
      Console.WriteLine("Inside Application");
      TemplateEngine.Instance.FindTemplate(dataContext).Render(dataContext);
    }
  }
}
