using System.Text;
using HelpDesk.Results.CustomResults;
using HelpDesk.Results.Services;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddSingleton<ITicketRepository, InMemoryTicketRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error/exception");
}

app.UseStatusCodePagesWithReExecute("/error/status/{0}");
app.UseStaticFiles();

app.Logger.LogInformation(
    "HelpDesk.Results started in {Environment} environment",
    app.Environment.EnvironmentName);

var homePage = """
<!doctype html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>HelpDesk.Results</title>
    <style>
        body { font-family: system-ui, sans-serif; margin: 2rem; line-height: 1.5; color: #17202a; }
        main { max-width: 880px; }
        h1 { margin-bottom: .25rem; }
        ul { columns: 2; padding-left: 1.2rem; }
        li { break-inside: avoid; margin: .35rem 0; }
        a { color: #0f6f8f; }
        code { background: #eef3f5; padding: .1rem .35rem; border-radius: .25rem; }
    </style>
</head>
<body>
<main>
    <h1>HelpDesk.Results</h1>
    <p>Minimal API demo for exception handling, HTTP status pages, Results API, logging, and a custom HTML result.</p>
    <ul>
        <li><a href="/about/text">/about/text</a></li>
        <li><a href="/about/content">/about/content</a></li>
        <li><a href="/api/tickets">/api/tickets</a></li>
        <li><a href="/api/tickets/1">/api/tickets/1</a></li>
        <li><a href="/api/tickets/999">/api/tickets/999</a></li>
        <li><a href="/api/tickets/create?title=Printer&amp;priority=2">/api/tickets/create?title=Printer&amp;priority=2</a></li>
        <li><a href="/api/tickets/create?priority=2">/api/tickets/create?priority=2</a></li>
        <li><a href="/status/unauthorized">/status/unauthorized</a></li>
        <li><a href="/status/forbidden">/status/forbidden</a></li>
        <li><a href="/status/custom/418">/status/custom/418</a></li>
        <li><a href="/redirect/old-tickets">/redirect/old-tickets</a></li>
        <li><a href="/redirect/ticket/1">/redirect/ticket/1</a></li>
        <li><a href="/files/readme">/files/readme</a></li>
        <li><a href="/throw">/throw</a></li>
        <li><a href="/unknown">/unknown</a></li>
    </ul>
</main>
</body>
</html>
""";

app.MapGet("/", () => Results.Extensions.Html(homePage));

app.MapGet("/about/text", () =>
    Results.Text("HelpDesk.Results returns this response through Results.Text()."));

app.MapGet("/about/content", () =>
    Results.Content(
        "HelpDesk.Results returns this response through Results.Content() with explicit UTF-8 text/plain content.",
        contentType: "text/plain",
        contentEncoding: Encoding.UTF8));

app.MapGet("/api/tickets", (ITicketRepository repository) =>
    Results.Json(repository.GetAll()));

app.MapGet("/api/tickets/{id:int}", (int id, ITicketRepository repository) =>
{
    var ticket = repository.GetById(id);

    return ticket is null
        ? Results.NotFound(new { message = $"Ticket {id} was not found." })
        : Results.Ok(ticket);
}).WithName("ticket-details");

app.MapGet(
    "/api/tickets/create",
    (string? title, int? priority, ITicketRepository repository, ILogger<Program> logger) =>
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            logger.LogWarning("Ticket creation rejected because title is missing");
            return Results.BadRequest(new { message = "Query parameter 'title' is required." });
        }

        var requestedPriority = priority ?? 3;
        if (requestedPriority is < 1 or > 5)
        {
            logger.LogWarning(
                "Ticket creation rejected because priority {Priority} is outside 1..5",
                requestedPriority);

            return Results.BadRequest(new { message = "Priority must be between 1 and 5." });
        }

        var ticket = repository.Create(title, requestedPriority);
        return Results.Created($"/api/tickets/{ticket.Id}", ticket);
    });

app.MapGet("/status/unauthorized", () => Results.Unauthorized());
app.MapGet("/status/forbidden", () => Results.StatusCode(StatusCodes.Status403Forbidden));
app.MapGet("/status/custom/418", () => Results.StatusCode(StatusCodes.Status418ImATeapot));

app.MapGet("/redirect/old-tickets", () => Results.LocalRedirect("/api/tickets"));
app.MapGet("/redirect/ticket/{id:int}", (int id) =>
    Results.RedirectToRoute("ticket-details", new { id }));

var readmePath = Path.Combine(
    app.Environment.WebRootPath ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot"),
    "files",
    "readme.txt");

app.MapGet("/files/readme", () =>
    Results.File(
        readmePath,
        contentType: "text/plain; charset=utf-8",
        fileDownloadName: "helpdesk-readme.txt"));

app.MapGet("/throw", (ILogger<Program> logger) =>
{
    logger.LogWarning("The /throw endpoint is about to raise a demo exception");
    throw new InvalidOperationException("Demo exception from /throw.");
});

app.MapGet("/error/exception", (HttpContext context, ILogger<Program> logger) =>
{
    var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    if (exceptionFeature?.Error is not null)
    {
        logger.LogError(
            exceptionFeature.Error,
            "Unhandled exception while processing {Path}",
            exceptionFeature.Path);
    }

    return Results.Json(
        new
        {
            status = StatusCodes.Status500InternalServerError,
            title = "Server error",
            message = "The request could not be processed. Please try again later."
        },
        statusCode: StatusCodes.Status500InternalServerError);
});

app.MapGet("/error/status/{code:int}", (int code, HttpContext context) =>
{
    var statusFeature = context.Features.Get<IStatusCodeReExecuteFeature>();

    return Results.Json(
        new
        {
            status = code,
            title = DescribeStatusCode(code),
            path = statusFeature?.OriginalPath ?? context.Request.Path.Value
        },
        statusCode: code);
});

app.Run();

static string DescribeStatusCode(int code) => code switch
{
    StatusCodes.Status400BadRequest => "Bad request",
    StatusCodes.Status401Unauthorized => "Unauthorized",
    StatusCodes.Status403Forbidden => "Forbidden",
    StatusCodes.Status404NotFound => "Not found",
    StatusCodes.Status418ImATeapot => "Custom status: I'm a teapot",
    StatusCodes.Status500InternalServerError => "Internal server error",
    >= 400 and < 500 => "Client error",
    >= 500 and < 600 => "Server error",
    _ => "HTTP status"
};
