using System.Net;

namespace Kanbardoo.Application.Results;

public class SuccessResult : Result
{
    public SuccessResult()
    {
        IsSuccess = true;
        HttpCode = HttpStatusCode.OK;
    }
}