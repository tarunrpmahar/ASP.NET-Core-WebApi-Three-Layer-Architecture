using ApiThreeLayerArch.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiThreeLayerArch.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                ApiError response;
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

                string message;
                var exceptionType = ex.GetType();

                if(exceptionType == typeof(UnauthorizedAccessException))
                {
                    statusCode = HttpStatusCode.Forbidden;
                    message = "You are not authorized";
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "some Unknown error occured";
                }

                if (env.IsDevelopment())
                {
                    response = new ApiError((int)statusCode, ex.Message, ex.StackTrace.ToString());
                }
                else
                {
                    response = new ApiError((int)statusCode, message);
                }

                logger.LogError(ex, $"Logging Exception - {message} - { exceptionType }");
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";

                //await context.Response.WriteAsync(ex.Message);
                await context.Response.WriteAsync(response.ToString());
            }
        }
    }
}
