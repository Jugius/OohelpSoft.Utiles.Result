
namespace OohelpSoft.Utiles.Result;
public interface IResult
{
    string ErrorMessage { get; }
    bool IsSuccess { get; }
    bool IsFailure { get; }
}
public interface IResult<out TValue> : IResult
{
    TValue Value { get; }
}
public interface IResultOrError<out TError> : IResult
{ 
    TError Error { get; }
}
public interface IResultOrError<out TValue, out TError> : IResult<TValue>, IResultOrError<TError>;