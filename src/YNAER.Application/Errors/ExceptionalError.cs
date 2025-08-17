using YNAER.Domain.Common;

namespace YNAER.Application.Errors;

public class ExceptionalError : Error
{
    public ExceptionalError(string message, Exception exception) : base(message, nameof(ExceptionalError))
    {
        Exception = exception;
    }

    public ExceptionalError(Exception exception) : base(exception.Message, nameof(ExceptionalError))
    {
        Exception = exception;
    }

    public Exception Exception { get; }
}