namespace Application.Common.Responses;

public class SuccessResponse<T> : BaseResponse<T>
{
    public SuccessResponse(T data)
    {
        Data = data;
    }
    
    public override bool IsSuccess { get; set; } = true;
}