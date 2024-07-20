using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace TelephoneShop
{
    public static class ErrorHandlingExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

                var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionDetails?.Error;

                logger.LogError(
                    exception,
                    "Couldn't process a request on machine {Machine}. TraceId: {TraceId}",
                    Environment.MachineName,
                    Activity.Current?.Id);

                await Results.Problem(
                    title: "Something went wrong",
                    statusCode: StatusCodes.Status500InternalServerError,
                    extensions: new Dictionary<string, object?>
                    {
                        {"traceId", Activity.Current?.Id}
                    }
                 ).ExecuteAsync(context);
            });
        }
    }
}
