
namespace OohelpSoft.Utiles.Result;
public class Result<TValue> : Result, IResult<TValue>
{
    public TValue Value { get; }   
    internal Result(TValue value) : base()
    {
        Value = value;
    }
    internal Result(Exception error) : base(error) 
    {        
        Value = default;
    }
    
    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<Exception, TResult> failure) =>
        IsSuccess ? success(Value) : failure(Error);

    public async Task<TResult> MatchAsync<TResult>(
        Func<TValue, Task<TResult>> onSuccess,
        Func<Exception, Task<TResult>> onFailure) =>
        IsSuccess ? await onSuccess(Value!) : await onFailure(Error);

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Exception error) => new(error);    
}
