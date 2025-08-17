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
                return new ApplicationValidationError(validationResult.Errors);
            }

            var account = await _repository.GetAsync(query.AccountId, ct);
            if (account is null)
            {
                return new NotFoundError(query.AccountId);
            }

            return account;
        }
        catch (Exception e)
        {
            return new ExceptionalError(e);
        }
    }
}