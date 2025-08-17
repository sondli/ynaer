using YNAER.Domain.Common;

namespace YNAER.Application.Abstractions.Common;

public interface IQueryHandler<TQuery, T> where TQuery : IQuery<T>
{
    public Task<Result<T>> HandleAsync(TQuery query, CancellationToken ct = default);
}