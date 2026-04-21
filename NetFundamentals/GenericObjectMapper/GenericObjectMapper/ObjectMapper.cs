using System.Reflection;

namespace GenericObjectMapper;

public static class ObjectMapper
{
    public static TDestination Map<TSource, TDestination>(TSource source) 
        where TDestination : new()
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        TDestination destination = new TDestination();

        PropertyInfo[] sourceProperties = source
            .GetType()
            .GetProperties();
        PropertyInfo[] destProperties = destination
            .GetType()
            .GetProperties();

        foreach (var sourceProperty in sourceProperties)
        {
            foreach (var destProperty in destProperties)
            {
                if (sourceProperty.Name == destProperty.Name 
                   && sourceProperty.PropertyType == destProperty.PropertyType
                   && destProperty.CanWrite)
                {
                    var value = sourceProperty.GetValue(source);
                    destProperty.SetValue(destination, value);
                }
            }
        }

        return destination;
    }
}
