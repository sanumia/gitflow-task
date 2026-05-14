using CustomModelBinders.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CustomModelBinders.ModelBinders;

public class PersonModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(Person))
        {
            return new PersonModelBinder();
        }

        return null;
    }
}
