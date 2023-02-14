using Kanbardoo.Application.Results;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Content { get; set; }
    public IEnumerable<string>? Errors { get; set; }

    protected Result()
    {

    }

    public static Result<T> SuccessResult(T content)
    {
        return new SuccessResult<T>(content);
    }

    public static Result<T> ErrorResult(IEnumerable<string> errors)
    {
        return new ErrorResult<T>(errors);
    }

    public static Result<T> ErrorResult(string error)
    {
        return ErrorResult(new List<string>() { error });
    }
}