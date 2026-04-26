using System.Text;

namespace UndoStringBuilder.Commands;

public class InsertCommand(StringBuilder receiver,
    int index,
    string text) : ICommand
{
    public void Execute() => receiver.Insert(index, text);
    public void Undo() => receiver.Remove(index, text.Length);
}
