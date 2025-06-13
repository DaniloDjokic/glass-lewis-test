
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/problem+json";
        var problemDetails = new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            title = "An unexpected error occurred!",
            status = 500,
            detail = "An internal server error has occurred."
        };
        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});
