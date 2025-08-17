using YNAER.Domain.Common;

namespace YNAER.Application.Errors;

public class NotFoundError : Error
{
    public NotFoundError(Guid resourceId) : base($"Resource with id {resourceId} was not found", nameof(NotFoundError))
    {
        ResourceId = resourceId;
    }

    public Guid ResourceId { get; }
}