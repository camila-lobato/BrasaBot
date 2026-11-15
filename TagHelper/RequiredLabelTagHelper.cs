using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BrasaBot.TagHelpers
{
    [HtmlTargetElement("label", Attributes = ForAttributeName)]
    public class RequiredLabelTagHelper : LabelTagHelper
    {
        private const string ForAttributeName = "asp-for";

        public RequiredLabelTagHelper(IHtmlGenerator generator) : base(generator) { }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Deixa o LabelTagHelper processar normalmente
            await base.ProcessAsync(context, output);

            // Depois verifica se a propriedade é obrigatória
            if (For?.ModelExplorer?.Metadata?.IsRequired == true)
            {
                // Adiciona a classe "required" no label
                var classes = output.Attributes.FirstOrDefault(a => a.Name == "class")?.Value?.ToString() ?? "";
                output.Attributes.SetAttribute("class", $"{classes} required".Trim());
            }
        }
    }
}