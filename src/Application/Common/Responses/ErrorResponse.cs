namespace Application.Common.Responses;

public class ErrorResponse<T> : BaseResponse<T>
{
    public ErrorResponse(string? message, IDictionary<string, string[]> errors = null!)
    {
        Errors = errors;
        Message = message;
    }

    public override bool IsSuccess { get; set; } = false;
}