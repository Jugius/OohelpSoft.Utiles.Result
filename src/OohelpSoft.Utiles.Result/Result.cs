
namespace OohelpSoft.Utiles.Result;
public class Result : IResultOrError<Exception>
{
    public Exception Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string ErrorMessage => Error?.GetBaseException().Message ?? string.Empty;

    protected Result(Exception error)
    {
        Error = error;
        IsSuccess = false;
    }
    protected Result()
    {        
        Error = default;
        IsSuccess = true;
    }
    
    public static implicit operator Result(Exception error) => new Result(error);
    public static Result Success() => new Result();
    public static Task<Result> SuccessAsync() => Task.FromResult(Success());
    public static Result Failure(Exception exception) => new Result(exception);
    public static Task<Result> FailureAsync(Exception exception) => Task.FromResult(Failure(exception));
    public static Result<T> Success<T>(T value) => new Result<T>(value);
    public static Task<Result<T>> SuccessAsync<T>(T value) => Task.FromResult(Success(value));
    public static Result<T> Failure<T>(Exception exception) => new Result<T>(exception);
    public static Task<Result<T>> FailureAsync<T>(Exception exception) => Task.FromResult(Failure<T>(exception));
    public TResult Match<TResult>(Func<TResult> onSuccess, Func<Exception, TResult> onFailure) =>
        IsSuccess ? onSuccess() : onFailure(Error);
    public async Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> onSuccess, Func<Exception, Task<TResult>> onFailure) =>
        IsSuccess ? await onSuccess() : await onFailure(Error);
}
