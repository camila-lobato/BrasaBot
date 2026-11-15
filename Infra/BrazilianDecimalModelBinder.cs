using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BrasaBot.Infra
{
    public class BrazilianDecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var valor = valueProviderResult.FirstValue;

            if (string.IsNullOrWhiteSpace(valor))
                return Task.CompletedTask;

            // Remove "R$", espaços e pontos de milhar
            // Ex: "R$ 1.234,56" → "1234,56"
            var valorLimpo = valor
                .Replace("R$", "")
                .Replace(" ", "")
                .Replace(".", "")   // Remove pontos de milhar
                .Replace(",", ".")  // Vírgula decimal → ponto
                .Trim();

            // Agora "1234,56" virou "1234.56" (formato universal)
            if (decimal.TryParse(valorLimpo, NumberStyles.Any, CultureInfo.InvariantCulture, out var resultado))
            {
                bindingContext.Result = ModelBindingResult.Success(resultado);
                return Task.CompletedTask;
            }

            bindingContext.ModelState.TryAddModelError(
                bindingContext.ModelName,
                "Valor inválido.");

            return Task.CompletedTask;
        }
    }
}