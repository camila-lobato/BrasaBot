using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BrasaBot.TagHelpers
{
    [HtmlTargetElement("label", Attributes = ForAttributeName)]
    public class RequiredLabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; } = null!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var isRequired = For?.ModelExplorer?.Metadata?.IsRequired == true;
            Console.WriteLine($">>> Label: {For?.Name}, IsRequired: {isRequired}");
            // Se o campo é obrigatório, adiciona a classe
            if (For?.ModelExplorer?.Metadata?.IsRequired == true)
            {
                var classes = output.Attributes.FirstOrDefault(a => a.Name == "class")?.Value?.ToString() ?? "";
                output.Attributes.SetAttribute("class", $"{classes} required".Trim());
            }
        }
    }
}