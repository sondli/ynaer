using FluentValidation;
using Microsoft.Extensions.Logging;
using YNAER.Application.Abstractions.Common;
using YNAER.Application.Errors;
using YNAER.Domain.Abstractions;
using YNAER.Domain.Common;
using YNAER.Domain.Entities;

namespace YNAER.Application.Features.BankAccounts.CreateBankAccount;

public class CreateBankAccountHandler : ICommandHandler<CreateBankAccountCommand, BankAccount>
{
    private readonly ILogger<CreateBankAccountHandler> _logger;
    private readonly IValidator<CreateBankAccountCommand> _validator;
    private readonly IBankAccountsRepository _repository;

    public CreateBankAccountHandler(ILogger<CreateBankAccountHandler> logger,
        IValidator<CreateBankAccountCommand> validator, IBankAccountsRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<BankAccount>> HandleAsync(CreateBankAccountCommand command, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Creating bank account with name {Name} for user {UserId}", command.Name,
                command.User.Id);

            var validationResult = await _validator.ValidateAsync(command, ct);
            if (!validationResult.IsValid)
            {
                return new ApplicationValidationError(validationResult.Errors);
            }

            var account = BankAccount.Create(command.User, command.Name);
            await _repository.AddAsync(account, ct);

            return account;
        }
        catch (Exception e)
        {
            return new ExceptionalError(e);
        }
    }
}