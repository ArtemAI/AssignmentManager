using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using NLog;
using PL.Models;
using System.Net;

namespace PL.Extensions
{
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
                    var error = new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Something went wrong. Please try again later."
                    };
                    await context.Response.WriteAsync(error.ToString());
                });
            });
        }
    }
}