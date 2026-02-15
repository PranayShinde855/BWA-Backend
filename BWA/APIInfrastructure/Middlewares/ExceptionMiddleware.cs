using System.Net;
using System.Text.Json;
using BWA.ServiceEntities;
using BWA.Utility;
using BWA.Utility.Exception;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BWA.APIInfrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var jsonOptions = context.RequestServices.GetService<IOptions<JsonOptions>>();
            context.Response.ContentType = "application/json";
            var exceptionData = GetExceptionDetails(exception);
            context.Response.StatusCode = (int)exceptionData.StatusCode;
            var json = JsonSerializer.Serialize(exceptionData, jsonOptions?.Value.SerializerOptions);
            await context.Response.WriteAsync(json);
        }
        private ResponseModel GetExceptionDetails(Exception exception)
        {
            string errorMessage = exception.Message;

            var model = new ResponseModel();
            var exceptionType = exception.GetType();

            if (IsTypeMatch<UnauthorizedAccessException>(exceptionType))
            {
                if (!string.IsNullOrEmpty(exception.Message) && exception.Message.Equals("MMP1015"))
                    errorMessage = exception.Message;

                SetResponse(ref model, errorMessage, HttpStatusCode.Unauthorized);
            }
            else if (IsTypeMatch<BadResultException>(exceptionType))
            {
                SetResponse(ref model, exception.Message, HttpStatusCode.BadRequest);
            }
            else if (IsTypeMatch<DuplicateRecordException>(exceptionType)
                || IsTypeMatch<System.ComponentModel.DataAnnotations.ValidationException>(exceptionType)
                || IsTypeMatch<RecordNotFoundException>(exceptionType)
                || IsTypeMatch<HttpRequestException>(exceptionType))
            {
                SetResponse(ref model, exception.Message, HttpStatusCode.PreconditionFailed);
            }
            else if (IsTypeMatch<PermissionResultException>(exceptionType))
            {
                SetResponse(ref model, exception.Message, HttpStatusCode.Forbidden);
            }
            else if (IsTypeMatch<SecurityTokenExpiredException>(exceptionType))
            {
                SetResponse(ref model, "Session timeout. Please login again.", HttpStatusCode.Unauthorized);
            }
            else
            {
                //if (IsTypeMatch<FluentValidation.ValidationException>(exceptionType))
                //{
                //    var errors = ((FluentValidation.ValidationException)exception)
                //        .Errors.Select(k => new Errors
                //        {
                //            PropertyName = k.PropertyName,
                //            ErrorMessages = ((FluentValidation.ValidationException)exception)
                //            .Errors.Select(p => p.ErrorMessage).ToArray()
                //        }).ToList();
                //    model.Message = "Bad Request";
                //    model.StatusCode = 400;
                //    model.Errors = errors;
                //}
                //else
                //{
                SetResponse(ref model, exception.Message, HttpStatusCode.InternalServerError);
                //}
            }
            return model;
        }
        private bool IsTypeMatch<T>(Type type) => type == typeof(T);
        private void SetResponse(ref ResponseModel model, string messageCode, HttpStatusCode httpStatusCode)
        {
            model.Message = ContentLoader.ReturnLanguageData(messageCode);
            model.StatusCode = (int)httpStatusCode;
        }
    }
}
