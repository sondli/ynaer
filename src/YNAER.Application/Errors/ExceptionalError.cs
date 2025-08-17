using YNAER.Domain.Common;

namespace YNAER.Application.Errors;

public class ExceptionalError : IError
{
    public ExceptionalError(string message, Exception exception)
    {
        Message = message;
        Exception = exception;
        Code = nameof(ExceptionalError);
    }

    public ExceptionalError(Exception exception)
    {
        Message = exception.Message;
        Exception = exception;
        Code = nameof(ExceptionalError);
    }

    public string Message { get; }
    public string Code { get; }
    public Exception Exception { get; }
}