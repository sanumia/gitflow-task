using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndoStringBuilder.Commands;

internal class ReplaceCommand(StringBuilder receiver,
    int startIndex,
    int length,
    string newText,
    string originalText) : ICommand
{
    public void Execute()
    {
        receiver.Remove(startIndex, length);
        receiver.Insert(startIndex, newText);
    }
    public void Undo()
    {
        receiver.Remove(startIndex, newText.Length);
        receiver.Insert(startIndex, originalText);
    }
}
