using YNAER.Domain.Common;

namespace YNAER.Application.Abstractions.Common;

public interface ICommandHandler<TCommand, T> where TCommand : ICommand<T>
{
    public Task<Result<T>> HandleAsync(TCommand command, CancellationToken ct = default);
}

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    public Task<Result> HandleAsync(TCommand command, CancellationToken ct = default);
}