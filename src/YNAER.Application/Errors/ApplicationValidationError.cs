using FluentValidation.Results;
using YNAER.Domain.Common;

namespace YNAER.Application.Errors;

public class ApplicationValidationError : IError
{
    private readonly IList<ValidationFailure> _validationFailures;

    public ApplicationValidationError(IList<ValidationFailure> validationFailures)
    {
        Message = "Validation failure";
        _validationFailures = validationFailures;
        Code = nameof(ApplicationValidationError);
    }

    public string Message { get; }
    public string Code { get; }
    public IReadOnlyList<ValidationFailure> ValidationFailures => _validationFailures.AsReadOnly();
}