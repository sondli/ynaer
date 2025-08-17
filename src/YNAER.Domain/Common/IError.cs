namespace YNAER.Domain.Common;

public interface IError
{
    public string Message { get; }
    public string Code { get; }
}