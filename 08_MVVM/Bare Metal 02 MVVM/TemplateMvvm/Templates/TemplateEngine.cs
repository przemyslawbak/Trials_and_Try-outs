using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateMvvm.Templates
{
  public class TemplateEngine
  {
    public static TemplateEngine Instance { get; } = new TemplateEngine();

    static TemplateEngine()
    {
    }

    private TemplateEngine()
    {
    }

    public List<TemplateBase> Templates { get; } = new List<TemplateBase> { new DefaultTemplate() };

    public void Add(TemplateBase template)
    {
      Templates.Add(template);
    }

    public TemplateBase FindTemplate<T>(T instance)
    {
      Type targetType = instance.GetType();
      while (true)
      {
        TemplateBase template = Templates.FirstOrDefault(x => x.TargetType == targetType);
        if (template != null)
        {
          return template;
        }
        targetType = targetType?.BaseType;
      }
    }
  }
}