namespace HelpDesk.Results.CustomResults;

public sealed class HtmlResult : IResult
{
    private readonly string _html;
    private readonly int _statusCode;

    public HtmlResult(string html, int statusCode = StatusCodes.Status200OK)
    {
        _html = html ?? throw new ArgumentNullException(nameof(html));
        _statusCode = statusCode;
    }

    public Task ExecuteAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Response.StatusCode = _statusCode;
        context.Response.ContentType = "text/html; charset=utf-8";

        return context.Response.WriteAsync(_html);
    }
}
