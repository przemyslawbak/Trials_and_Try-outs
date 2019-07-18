using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    [HtmlTargetElement(Attributes = CustomBackground)] //contains "customcolor" attribute
    [HtmlTargetElement(Attributes = TheName)] //contains "customcolor" attribute
    public class CustomColorTagHelper : TagHelper
    {
        private const string CustomBackground = "custombackground";
        private const string TheName = "customcolor";
        [HtmlAttributeName(TheName)]
        public string TheValue { get; set; } //helpers property
        [HtmlAttributeName(CustomBackground)]
        public string TheBackground { get; set; } //helpers property
        private ApplicationDbContext _context;
        public CustomColorTagHelper(ApplicationDbContext ctx)
        {
            _context = ctx;
        }
       public override void Process(TagHelperContext context, TagHelperOutput output)
       {
            var colorBack = "";
            var colorStyle = "";
            var styleString = "";
            var colors = _context.ColorPanels.FirstOrDefault(); //context
            if (TheBackground == "one")
            {
                colorBack = "background-color:" + "grey" + "; "; //new color
            }
            if (TheBackground == "two")
            {
                colorBack = "background-color:" + "yellow" + "; "; //new color
            }
            if (TheValue == "one")
            {
                colorStyle = "color:" + "pink" + "; "; //new color
            }
            if (TheValue == "two")
            {
                colorStyle = "color:" + "cyan" + "; "; //new color
            }
            styleString = colorStyle + colorBack;
            if (!output.Attributes.ContainsName("style"))
            {
                output.Attributes.SetAttribute("style", styleString);
            }
            else
            {
                var currentAttribute = output.Attributes.FirstOrDefault(attribute => attribute.Name == "style"); //get value of 'style'
                string newAttributeValue = $"{currentAttribute.Value.ToString() + "; " + styleString}"; //combine style values
                output.Attributes.Remove(currentAttribute); //remove old attribute
                output.Attributes.SetAttribute("style", newAttributeValue); //add merged attribute values
            }
        }
    }
}
