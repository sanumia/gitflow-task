using CsvHelper;
using System.Collections;
using System.Globalization;

namespace Serialization;

public class CsvEnumerable<T>(string filePath) : IEnumerable<T>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator()
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        foreach (var item in csv.GetRecords<T>())
            yield return item;
    }
}
