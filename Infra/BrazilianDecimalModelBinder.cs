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

            // Remove "R$", espaços
            valor = valor.Replace("R$", "").Replace(" ", "").Trim();

            // Remove pontos de milhar ANTES de converter vírgula
            // Mas só remove pontos se houver vírgula (indica formato pt-BR: "1.234,56")
            if (valor.Contains(","))
            {
                valor = valor.Replace(".", "").Replace(",", ".");
            }
            // Se NÃO tiver vírgula, assume que é formato universal (0.2, 1234.56)

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out var resultado))
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