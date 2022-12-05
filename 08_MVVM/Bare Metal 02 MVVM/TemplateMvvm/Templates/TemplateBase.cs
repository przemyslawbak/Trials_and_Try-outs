using System;

namespace TemplateMvvm.Templates
{
  public abstract class TemplateBase
  {
    public Type TargetType { get; set; }

    public abstract void Render(dynamic dataContext);
  }
}