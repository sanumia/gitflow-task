using ImpromptuInterface;
using LoggingProxy.Interfaces;
using System.Dynamic;

namespace LoggingProxy;

public class LoggingProxy<T>(ILogger logger) : DynamicObject
    where T : class
{
    private T _target;

    public T CreateInstance(T obj)
    {
        _target = obj;

        return this.ActLike<T>();
    }

    public override bool TryInvokeMember(
        InvokeMemberBinder binder,
        object?[]? args,
        out object? result)
    {
        logger.Log($"Calling {binder.Name}({string.Join(", ", args)})");

        var method = _target
            .GetType()
            .GetMethod(binder.Name);
        result = method.Invoke(_target, args);

        logger.Log($"Done {binder.Name} => {result}");

        return true;
    }
}
