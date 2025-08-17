using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using YNAER.Application.Features.BankAccounts.CreateBankAccount;
using YNAER.Application.Features.BankAccounts.DeleteBankAccount;
using YNAER.Application.Features.BankAccounts.GetBankAccount;
using YNAER.Application.Features.BankAccounts.ListBankAccounts;

namespace YNAER.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IValidator<GetBankAccountQuery>, GetBankAccountValidator>();
        services.AddScoped<IValidator<CreateBankAccountCommand>, CreateBankAccountValidator>();
        services.AddScoped<IValidator<DeleteBankAccountCommand>, DeleteBankAccountValidator>();
        services.AddScoped<GetBankAccountHandler>();
        services.AddScoped<ListBankAccountsHandler>();
        services.AddScoped<CreateBankAccountHandler>();
        services.AddScoped<DeleteBankAccountHandler>();

        return services;
    }
}