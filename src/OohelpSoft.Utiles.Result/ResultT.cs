using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OohelpSoft.Utiles.Result;
public class Result<TValue> : Result, IResultOrError<TValue, Exception>
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
    /// <summary>
    /// Executes the corresponding action based on whether the operation succeeded.
    /// </summary>
    /// <param name="onSuccess">Action to execute on success (receives the data).</param>
    /// <param name="onFailure">Action to execute on failure (receives the error message).</param>
    public TResult Match<TResult>(
        Func<TValue, TResult> onSuccess,
        Func<Exception, TResult> onFailure) =>
        IsSuccess ? onSuccess(Value) : onFailure(Error);


    /// <summary>
    /// Asynchronously executes the corresponding action based on whether the operation succeeded.
    /// </summary>
    /// <param name="onSuccess">Async action to execute on success (receives the data).</param>
    /// <param name="onFailure">Async action to execute on failure (receives the error message).</param>
    public async Task<TResult> MatchAsync<TResult>(
        Func<TValue, Task<TResult>> onSuccess,
        Func<Exception, Task<TResult>> onFailure) =>
        IsSuccess ? await onSuccess(Value!) : await onFailure(Error);


    /// <summary>
    /// Maps the data contained in the result to a new type.
    /// </summary>
    /// <typeparam name="TResult">The new data type.</typeparam>
    /// <param name="map">Mapping function.</param>
    public Result<TResult> Map<TResult>(Func<TValue, TResult> map) where TResult : class
        => IsSuccess ? Result.Success(map(Value!)) : Result.Failure<TResult>(Error);


    /// <summary>
    /// Asynchronously maps the data contained in the result to a new type.
    /// </summary>
    /// <typeparam name="TResult">The new data type.</typeparam>
    /// <param name="map">Async mapping function.</param>
    public async Task<Result<TResult>> MapAsync<TResult>(Func<TValue, Task<TResult>> map)
        => IsSuccess ? Result.Success(await map(Value!)) : await Result.FailureAsync<TResult>(Error);

    /// <summary>
    /// Binds the result to another result, enabling chained operations.
    /// </summary>
    /// <typeparam name="TResult">The data type of the resulting result.</typeparam>
    /// <param name="bind">Binding function that returns a new result.</param>
    public Result<TResult> Bind<TResult>(Func<TValue, Result<TResult>> bind)
        => IsSuccess ? bind(Value!) : Result.Failure<TResult>(Error);

    /// <summary>
    /// Asynchronously binds the result to another result, enabling chained operations.
    /// </summary>
    /// <typeparam name="TResult">The data type of the resulting result.</typeparam>
    /// <param name="bind">Async binding function that returns a new result.</param>
    public Task<Result<TResult>> BindAsync<TResult>(Func<TValue, Task<Result<TResult>>> bind)
        => IsSuccess ? bind(Value!) : Result.FailureAsync<TResult>(Error);

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Exception error) => new(error);    
}
