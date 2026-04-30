namespace UndoStringBuilder;

public interface ICommand
{
    void Execute();
    void Undo();
}
