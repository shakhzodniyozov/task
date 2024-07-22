namespace Application.Common.Responses;

public abstract class BaseResponse<T>
{
    public T? Data { get; set; }
    public virtual bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
}