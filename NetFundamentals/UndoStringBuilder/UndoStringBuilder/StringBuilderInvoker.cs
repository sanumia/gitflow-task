using System.Text;
using UndoStringBuilder.Commands;

namespace UndoStringBuilder;

public class StringBuilderInvoker(string? initial = null, int? capacity = null)
{
    private readonly StringBuilder _builder = (initial, capacity) switch
    {
        (not null, not null) => new StringBuilder(initial, capacity.Value),
        (not null, null) => new StringBuilder(initial),
        (null, not null) => new StringBuilder(capacity.Value),
        (null, null) => new StringBuilder()
    };

    private readonly Stack<ICommand> _history = new();

    private void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public StringBuilderInvoker Append(string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        var command = new AppendCommand(_builder, value, _builder.Length);
        ExecuteCommand(command);

        return this;
    }

    public StringBuilderInvoker Append(char value) => Append(value.ToString());

    public StringBuilderInvoker AppendLine() => Append(Environment.NewLine);

    public StringBuilderInvoker AppendLine(string value) => Append(value + Environment.NewLine);

    public StringBuilderInvoker Insert(int index, string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (index < 0 || index > _builder.Length)
            throw new ArgumentOutOfRangeException(nameof(index));
        var command = new InsertCommand(_builder, index, value);
        ExecuteCommand(command);

        return this;
    }

    public StringBuilderInvoker Insert(int index, char value) => Insert(index, value.ToString());

    public StringBuilderInvoker Remove(int startIndex, int length)
    {
        string removed = ValidateRange(startIndex, length);
        var command = new RemoveCommand(_builder, startIndex, length, removed);
        ExecuteCommand(command);

        return this;
    }

    public StringBuilderInvoker Replace(int startIndex, int length, string newValue)
    {
        string original = ValidateRange(startIndex, length);
        var command = new ReplaceCommand(_builder, startIndex, length, newValue, original);
        ExecuteCommand(command);

        return this;
    }

    public StringBuilderInvoker Clear()
    {
        if (_builder.Length == 0) return this;
        string previous = _builder.ToString();
        var command = new ClearCommand(_builder, previous);
        ExecuteCommand(command);

        return this;
    }

    public void Undo()
    {
        if (_history.Count > 0)
        {
            var command = _history.Pop();
            command.Undo();
        }
    }

    private string ValidateRange(int startIndex, int length)
    {
        if (startIndex < 0 || startIndex >= _builder.Length)
            throw new ArgumentOutOfRangeException(nameof(startIndex));
        if (length < 0 || startIndex + length > _builder.Length)
            throw new ArgumentOutOfRangeException(nameof(length));
        return _builder.ToString(startIndex, length);
    }

    public override string ToString() => _builder.ToString();

    public int Length => _builder.Length;
}
