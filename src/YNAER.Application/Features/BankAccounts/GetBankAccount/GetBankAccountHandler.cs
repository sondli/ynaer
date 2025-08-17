using FluentValidation;
using Microsoft.Extensions.Logging;
using YNAER.Application.Abstractions.Common;
using YNAER.Application.Errors;
using YNAER.Domain.Abstractions;
using YNAER.Domain.Common;
using YNAER.Domain.Entities;

namespace YNAER.Application.Features.BankAccounts.GetBankAccount;

public class GetBankAccountHandler : IQueryHandler<GetBankAccountQuery, BankAccount>
{
    private readonly ILogger<GetBankAccountHandler> _logger;
    private readonly IBankAccountsRepository _repository;
    private readonly IValidator<GetBankAccountQuery> _validator;

    public GetBankAccountHandler(ILogger<GetBankAccountHandler> logger, IBankAccountsRepository repository,
        IValidator<GetBankAccountQuery> validator)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<BankAccount>> HandleAsync(GetBankAccountQuery query, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Getting bank account with id {Id}", query.AccountId);

            var validationResult = await _validator.ValidateAsync(query, ct);
            if (!validationResult.IsValid)
            {
                var error = new ApplicationValidationError(validationResult.Errors);
                return Result<BankAccount>.Fail(error);
            }

            var account = await _repository.GetAsync(query.AccountId, ct);
            if (account is null)
            {
                var error = new NotFoundError(query.AccountId);
                return Result<BankAccount>.Fail(error);
            }

            return account;
        }
        catch (Exception e)
        {
            var error = new ExceptionalError(e);
            return Result<BankAccount>.Fail(error);
        }
    }
}