using CustomJsonFormatter.Formatters;

namespace CustomJsonFormatter.Extensions;

public static class CustomJsonFormatterExtension
{
    public static IMvcBuilder AddCustomJsonFormatter(this IMvcBuilder builder)
    {
        builder.AddMvcOptions(options =>
        {
            options.OutputFormatters.Insert(0, new CustomJsonLinksFormatter());
        });

        return builder;
    }
}
