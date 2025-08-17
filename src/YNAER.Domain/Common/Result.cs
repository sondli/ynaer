namespace YNAER.Domain.Common;

public class Result
{
    private readonly Error? _error;

    protected Result(Error? error)
    {
        _error = error;
    }


    public bool IsSuccess => _error is null;
    public bool IsFailure => !IsSuccess;

    public Error Error =>
        _error ?? throw new InvalidOperationException("Cannot access the error of a successful result.");

    public static Result Ok()
    {
        return new Result(null);
    }

    public static Result Fail(Error error)
    {
        return new Result(error);
    }
    
    public static implicit operator Result(Error error) => Fail(error);
}

public class Result<T> : Result
{
    private readonly T? _value;

    private Result(T value) : base(null)
    {
        _value = value;
    }

    private Result(Error error) : base(error)
    {
    }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access the value of a failed result.");

    public static Result<T> Ok(T value)
    {
        return new Result<T>(value);
    }

    public new static Result<T> Fail(Error error)
    {
        return new Result<T>(error);
    }

    public static implicit operator Result<T>(T value) => Ok(value);
    public static implicit operator Result<T>(Error error) => Fail(error);
}