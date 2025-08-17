namespace YNAER.Domain.Common;

public class Error : IError
{
    public Error(string message, string code)
    {
        Message = message;
        Code = code;
    }

    public string Message { get; }
    public string Code { get; }
}