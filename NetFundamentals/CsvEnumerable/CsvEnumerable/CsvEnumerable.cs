using CsvHelper;
using System.Collections;
using System.Globalization;

namespace CsvEnumerable;

public class CsvEnumerable<T>(string filePath) : IEnumerable<T>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<T> GetEnumerator()
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<T>();
            foreach (var item in records)
            {
                yield return item;
            }
        }

    }
}
