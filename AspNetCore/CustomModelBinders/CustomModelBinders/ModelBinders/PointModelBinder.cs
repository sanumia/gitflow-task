using CustomModelBinders.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CustomModelBinders.ModelBinders;

public class PointModelBinder : IModelBinder
{
    private const int CoordinatesLength = 3;
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var value = bindingContext.ValueProvider.GetValue("coord").FirstValue;

        if (string.IsNullOrEmpty(value))
            return Task.CompletedTask;

        var parts = value.Split(',');

        if (parts.Length != CoordinatesLength)
            return Task.CompletedTask;

        if (int.TryParse(parts[0], out int x) 
            && int.TryParse(parts[1], out int y) 
            && int.TryParse(parts[2], out int z))
        {
            var point = new Point
            {
                X = x,
                Y = y,
                Z = z
            };

            bindingContext.Result = ModelBindingResult.Success(point);
        }

        return Task.CompletedTask;
    }
}
