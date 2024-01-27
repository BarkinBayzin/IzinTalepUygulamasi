using System.Text.Json.Serialization;
public class BasePaginationResponse<T>
{

    public bool Success { get; set; } = false;
    public string Message { get; set; }

    public T Data { get; set; }

    public int TotalCount { get; set; }

    public int CurrentPageIndex { get; set; }
    public int PageSize { get; set; } = 10;
    public int PageCount => (TotalCount + PageSize - 1) / PageSize;

    [JsonConstructor]
    public BasePaginationResponse()
    {
    }

    public BasePaginationResponse(bool success, string message = "")
    {
        Success = success;
        Message = message;
    }

    public BasePaginationResponse(T data, bool success, string message = "") : this(success, message)
    {
        Data = data;
    }

    public BasePaginationResponse(T data, bool success, string message, int totalCount) : this(data, success, message)
    {
        TotalCount = totalCount;
    }

    public BasePaginationResponse(T data, bool success, string message, int currentPageIndex, int totalCount) : this(data, success, message, totalCount)
    {
        CurrentPageIndex = currentPageIndex;
    }

    public BasePaginationResponse(T data, bool success, string message, int currentPageIndex, int totalCount, int pageSize) : this(data, success, message, currentPageIndex, totalCount)
    {
        PageSize = pageSize;
    }
}
