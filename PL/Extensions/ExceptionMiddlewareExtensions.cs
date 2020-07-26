using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using NLog;

namespace PL.Extensions
{
    /// <summary>
    /// Catches all unhandled exceptions and modifies response to request.
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    Logger.Error(contextFeature.Error);
                    await context.Response.WriteAsync("Something went wrong. Please try again later.");
                });
            });
        }
    }
}