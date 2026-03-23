namespace StudentPortal.Diagnostics.Middleware;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _validToken;

    public TokenMiddleware(RequestDelegate next, string validToken)
    {
        _next = next;
        _validToken = validToken;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Query["token"].ToString();

        if (string.IsNullOrEmpty(token) || token != _validToken)
        {
            context.Response.StatusCode = 403;
            return;
        }

        await _next(context);
    }
}