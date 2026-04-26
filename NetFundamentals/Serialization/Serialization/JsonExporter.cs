using System.Runtime.Serialization.Json;

namespace Serialization;

public static class JsonExporter
{
    public static void Export<T>(IEnumerable<T> data, string outputPath)
    {
        var list = data.ToList();
        var serializer = new DataContractJsonSerializer(typeof(List<T>));

        using var stream = File.Create(outputPath);
        serializer.WriteObject(stream, list);

        Console.WriteLine($"  Exported {list.Count} record(s) → {outputPath}");
    }
}
