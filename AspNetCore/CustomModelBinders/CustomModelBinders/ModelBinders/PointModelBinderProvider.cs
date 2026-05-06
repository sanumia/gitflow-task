using CustomModelBinders.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CustomModelBinders.ModelBinders;

public class PointModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(Point))
        {
            return new PointModelBinder();
        }

        return null;
    }
}
