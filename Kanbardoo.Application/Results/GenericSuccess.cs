public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T content)
    {
        IsSuccess = true;
        Content = content;
    }
}