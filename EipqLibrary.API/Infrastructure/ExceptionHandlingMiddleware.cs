using System;
using System.Dynamic;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;
using EipqLibrary.Shared.CustomExceptions;
using Microsoft.AspNetCore.Http;

namespace EipqLibrary.API.Infrastructure
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                dynamic responseBody = new ExpandoObject();
                responseBody.hasError = true;
                responseBody.errorMessage = error.Message;

                switch (error)
                {
                    case EntityNotFoundException e:
                        {
                            response.StatusCode = (int)HttpStatusCode.OK;
                            responseBody.actualStatusCode = (int)HttpStatusCode.NotFound;
                            break;
                        }
                    case BadDataException e:
                        {
                            response.StatusCode = (int)HttpStatusCode.OK;
                            responseBody.actualStatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        }
                    case UnauthorizedAccessException e:
                        {
                            responseBody.errorMessage = e.Message;
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            break;
                        }
                    case AuthenticationException e:
                        {
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        }
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize((object)responseBody);
                await response.WriteAsync(result);
            }
        }
    }
}
