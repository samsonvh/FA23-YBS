using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using YBS.Service.Dtos.PageResponses;
using YBS.Service.Exceptions;

namespace YBS.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly IMapper _mapper;
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, IMapper mapper)
        {
            _next = next;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (AggregateAPIException ex)
            {

                _logger.LogError($"Error: {ex.Message}");
                //Set up the response status code 
                context.Response.StatusCode = ex.StatusCode;
                //Set up the response type to Json
                context.Response.ContentType = "application/json";
                //map from List<APIException> to List<CommonErrorException>
                var errorResponseList = _mapper.Map<List<CommonErrorResponse>>(ex.Exceptions);
                //create new object {statusCode, Message, List<CommonErrorResponse>}
                var finalResponse = new { ex.StatusCode, ex.Message, errorResponseList };
                //Create API Exception and serialize to Json 
                var result = JsonConvert.SerializeObject(finalResponse);
                //Write error json to response body 
                await context.Response.WriteAsync(result);

            }
            catch (AggregateValidateAPIException ex)
            {

                _logger.LogError($"Error: {ex.Message}");
                //Set up the response status code 
                context.Response.StatusCode = ex.StatusCode;
                //Set up the response type to Json
                context.Response.ContentType = "application/json";
                //map from List<APIException> to List<ValidateErrorResponse>
                var errorResponseList = _mapper.Map<List<ValidateErrorResponse>>(ex.Exceptions);
                //create new object {statusCode, Message, List<ValidateErrorResponse>}
                var finalResponse = new { ex.StatusCode, ex.Message, errorResponseList };
                //Create API Exception and serialize to Json 
                var result = JsonConvert.SerializeObject(finalResponse);
                //Write error json to response body 
                await context.Response.WriteAsync(result);

            }
            catch (SingleAPIException ex)
            {

                _logger.LogError($"Error: {ex.Message}");
                //Set up the response status code 
                context.Response.StatusCode = ex.StatusCode;
                //Set up the response type to Json
                context.Response.ContentType = "application/json";
                //create new object {statusCode, Message, List<CommonErrorResponse>}
                var finalResponse = new { ex.StatusCode, ex.Message };
                //Create API Exception and serialize to Json 
                var result = JsonConvert.SerializeObject(finalResponse);
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
                var exception = new APIException(ex.Message).ToJson();
                //Write error json to response body
                await context.Response.WriteAsync(exception);
            }
        }

    }
}