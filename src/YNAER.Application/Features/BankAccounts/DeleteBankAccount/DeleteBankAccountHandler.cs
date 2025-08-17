using FluentValidation;
using Microsoft.Extensions.Logging;
using YNAER.Application.Abstractions.Common;
using YNAER.Application.Errors;
using YNAER.Domain.Abstractions;
using YNAER.Domain.Common;

namespace YNAER.Application.Features.BankAccounts.DeleteBankAccount;

public class DeleteBankAccountHandler : ICommandHandler<DeleteBankAccountCommand>
{
    private readonly ILogger<DeleteBankAccountHandler> _logger;
    private readonly IValidator<DeleteBankAccountCommand> _validator;
    private readonly IBankAccountsRepository _repository;

    public DeleteBankAccountHandler(ILogger<DeleteBankAccountHandler> logger,
        IValidator<DeleteBankAccountCommand> validator, IBankAccountsRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result> HandleAsync(DeleteBankAccountCommand command, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Deleting bank account with id {Id}", command.Id);

            var validationResult = await _validator.ValidateAsync(command, ct);
            if (!validationResult.IsValid)
            {
                var error = new ApplicationValidationError(validationResult.Errors);
                return Result.Fail(error);
            }

            var account = await _repository.GetAsync(command.Id, ct);
            if (account is null)
            {
                var error = new NotFoundError(command.Id);
                return Result.Fail(error);
            }

            await _repository.DeleteAsync(account, ct);
            return Result.Ok();
        }
        catch (Exception e)
        {
            var error = new ExceptionalError(e);
            return Result.Fail(error);
        }
    }
}