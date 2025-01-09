using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class LongBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        var value = valueProviderResult.FirstValue;

        // Remove formatação e caracteres não numéricos
        value = Regex.Replace(value, @"\D", "");

        if (long.TryParse(value, out long longResult))
        {
            bindingContext.Result = ModelBindingResult.Success(longResult);
        }
        else if (int.TryParse(value, out int intResult))
        {
            bindingContext.Result = ModelBindingResult.Success((long)intResult);
        }
        else
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "O valor informado não é válido.");
        }

        return Task.CompletedTask;
    }
}