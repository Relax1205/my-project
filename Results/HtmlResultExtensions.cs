namespace HelpDesk.Results.CustomResults;

public static class HtmlResultExtensions
{
    public static IResult Html(
        this IResultExtensions extensions,
        string html,
        int statusCode = StatusCodes.Status200OK)
    {
        ArgumentNullException.ThrowIfNull(extensions);

        return new HtmlResult(html, statusCode);
    }
}
