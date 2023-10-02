using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YBS.Data.Responses;

namespace YBS.Services.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (APIException ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                //Set up the response status code 
                context.Response.StatusCode = ex.StatusCode;
                //Set up the response type to Json
                context.Response.ContentType = "application/json";
                //Create API Exception and serialize to Json 
                var exception = new {ex.StatusCode,ex.Message};
                var result = JsonConvert.SerializeObject(exception);
                //Write error json to response body 
                await context.Response.WriteAsync(result);
              
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Unhadled exception : {ex}");
                //Set up the response status code 
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //Set up the response type to Json
                context.Response.ContentType = "application/json";
                //Create API Exception and serialize to Json 
                var exception = new APIException(context.Response.StatusCode,ex.Message).ToJson();
                //Write error json to response body
                await context.Response.WriteAsync (exception);
            }
        }

    }
}