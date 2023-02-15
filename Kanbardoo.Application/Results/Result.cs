namespace Kanbardoo.Application.Results;

public class Result
{
    public bool IsSuccess { get; set; }
    public IEnumerable<string>? Errors { get; set; }

    protected Result()
    {

    }

    public static Result SuccessResult()
    {
        return new SuccessResult();
    }

    public static Result ErrorResult(IEnumerable<string> errors)
    {
        return new ErrorResult(errors);
    }

    public static Result ErrorResult(string error)
    {
        return new ErrorResult(new List<string> { error });
    }
}