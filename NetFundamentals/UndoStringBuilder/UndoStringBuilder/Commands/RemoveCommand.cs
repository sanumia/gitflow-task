using System.Text;

namespace UndoStringBuilder.Commands;

public class RemoveCommand(StringBuilder receiver,
    int startIndex,
    int length,
    string removedText) : ICommand
{
    public void Execute() => receiver.Remove(startIndex, length);
    public void Undo() => receiver.Insert(startIndex, removedText);
}
