using System.Text;

namespace UndoStringBuilder.Commands;

public class ClearCommand(StringBuilder receiver, string previousContent) : ICommand
{
    public void Execute() => receiver.Clear();
    public void Undo()
    {
        receiver.Clear();
        receiver.Append(previousContent);
    }
}
