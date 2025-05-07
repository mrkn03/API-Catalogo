using System.Net;
using APICatalogo.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace APICatalogo.Extensions
{
    public static class ApiExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature != null)
                    {
                        var errorDetails = new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = exceptionHandlerPathFeature.Error.Message,
                            Trace = exceptionHandlerPathFeature.Error.StackTrace
                        };
                        await context.Response.WriteAsync(errorDetails.ToString());
                    }
                });
            });
        }
    }
}
