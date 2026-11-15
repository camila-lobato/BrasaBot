using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BrasaBot.Infra
{
    public class BrazilianDecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(decimal) || context.Metadata.ModelType == typeof(decimal?))
            {
                return new BrazilianDecimalModelBinder();
            }
            return null;
        }
    }
}