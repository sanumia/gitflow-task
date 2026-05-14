using CustomModelBinders.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace CustomModelBinders.ModelBinders;

public class PersonModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var value = bindingContext.ValueProvider.GetValue("id").FirstValue;

        if (string.IsNullOrEmpty(value))
            return Task.CompletedTask;

        try
        {
            var bytes = Convert.FromBase64String(value);
            var decodedString = Encoding.UTF8.GetString(bytes);

            if (Guid.TryParse(decodedString, out Guid id))
            {
                var person = new Person
                {
                    Id = id,
                    Name = "Test User",
                    Age = 30
                };

                bindingContext.Result = ModelBindingResult.Success(person);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error decoding Base64 person ID", ex);
        }

        return Task.CompletedTask;
    }
}
