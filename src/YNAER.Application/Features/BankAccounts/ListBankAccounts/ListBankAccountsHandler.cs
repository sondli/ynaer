using Microsoft.Extensions.Logging;
using YNAER.Application.Abstractions.Common;
using YNAER.Application.Errors;
using YNAER.Domain.Abstractions;
using YNAER.Domain.Common;
using YNAER.Domain.Entities;

namespace YNAER.Application.Features.BankAccounts.ListBankAccounts;

public class ListBankAccountsHandler : IQueryHandler<ListBankAccountsQuery, IEnumerable<BankAccount>>
{
    private readonly ILogger<ListBankAccountsHandler> _logger;
    private readonly IBankAccountsRepository _repository;

    public ListBankAccountsHandler(ILogger<ListBankAccountsHandler> logger,
        IBankAccountsRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<IEnumerable<BankAccount>>> HandleAsync(ListBankAccountsQuery query,
        CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Listing bank accounts for user {UserId}", query.User.Id);

            var accounts = await _repository.ListByUserIdAsync(query.User.Id, ct);
            return Result<IEnumerable<BankAccount>>.Ok(accounts);
        }
        catch (Exception e)
        {
            return new ExceptionalError(e);
        }
    }
}