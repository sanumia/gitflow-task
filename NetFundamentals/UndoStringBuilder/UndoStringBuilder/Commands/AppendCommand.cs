using System.Text;

namespace UndoStringBuilder.Commands;

public class AppendCommand(
    StringBuilder receiver,
    string text,
    int startIndex) : ICommand
{
    public void Execute() => receiver.Append(text);
    public void Undo() => receiver.Remove(startIndex, text.Length);
}
