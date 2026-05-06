using System.Text;

namespace CustomModelBinders.Helpers;

public class PersonHelper
{
    public static (Guid guid, string base64) Generate()
    {
        var guid = Guid.NewGuid();
        var base64 = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(guid.ToString())
        );

        return (guid, base64);
    }

    public static void Print()
    {
        var (guid, base64) = Generate();

        Console.WriteLine(guid);
        Console.WriteLine(base64);
    }
}
