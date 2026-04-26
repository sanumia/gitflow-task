namespace UndoStringBuilder;

class Program
{
    static void Main()
    {
        var sb = new StringBuilderInvoker("test");
        sb.Append("1")
          .Append("2")
          .Append("3");
        sb.Undo();
        sb.Undo();
        Console.WriteLine(sb.ToString());
    }
}