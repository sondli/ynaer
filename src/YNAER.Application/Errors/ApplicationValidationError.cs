using FluentValidation.Results;
using YNAER.Domain.Common;

namespace YNAER.Application.Errors;

public class ApplicationValidationError : Error
{
    private readonly IList<ValidationFailure> _validationFailures;

    public ApplicationValidationError(IList<ValidationFailure> validationFailures)
        : base("Validation failure", nameof(ApplicationValidationError))
    {
        _validationFailures = validationFailures;
    }

    public IReadOnlyList<ValidationFailure> ValidationFailures => _validationFailures.AsReadOnly();
}