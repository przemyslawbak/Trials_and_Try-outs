using System;

namespace TemplateMvvm.Templates
{
  public class DefaultTemplate : TemplateBase
  {
    public DefaultTemplate()
    {
      TargetType = typeof (object);
    }

    public override void Render(dynamic dataContext)
    {
      Console.WriteLine($"{dataContext}");
    }
  }
}