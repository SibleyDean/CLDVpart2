public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    // ? Error Details (Only used for debugging)
    public bool ShowDetails { get; set; } = false;
    public string? ErrorMessage { get; set; }
    public string? StackTrace { get; set; }
}
