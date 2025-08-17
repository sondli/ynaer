using YNAER.Domain.Common;

namespace YNAER.Application.Errors;

public class NotFoundError : IError
{
    public NotFoundError(Guid resourceId)
    {
        ResourceId = resourceId;
        Message = $"Resource with id {resourceId} was not found";
        Code = nameof(NotFoundError);
    }

    public string Message { get; }
    public string Code { get; }
    public Guid ResourceId { get; }
}