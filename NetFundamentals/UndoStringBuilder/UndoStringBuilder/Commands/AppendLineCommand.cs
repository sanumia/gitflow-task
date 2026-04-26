using System.Text;

namespace UndoStringBuilder.Commands;

public class AppendLineCommand(
    StringBuilder receiver,
    string textWithNewLine,
    int startIndex) : ICommand 
{
    public void Execute() => receiver.Append(textWithNewLine);
    public void Undo() => receiver.Remove(startIndex, textWithNewLine.Length);
}
