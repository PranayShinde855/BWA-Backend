using BWA.ServiceEntities;
using BWA.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BWA.APIInfrastructure.Filters
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
                return;

            if (context.Result.GetType() == typeof(FileContentResult))
                return;

            var result = context.Result;
            var responseObj = new ResponseModel
            {
                Message = string.Empty,
                StatusCode = 200,
                Errors = null,
            };

            switch (result)
            {
                case OkObjectResult okresult:
                    responseObj.Data = okresult.Value;
                    break;
                case ObjectResult objectResult:
                    var data = (Dictionary<string, object>)(objectResult.Value);
                    responseObj.Message = data.ContainsKey(Constants.RESPONSE_MESSAGE_FIELD) ? Convert.ToString(data[Constants.RESPONSE_MESSAGE_FIELD]) : null;
                    responseObj.Data = data.ContainsKey(Constants.RESPONSE_DATA_FIELD) ? data[Constants.RESPONSE_DATA_FIELD] : null;
                    break;
                case JsonResult json:
                    responseObj.Data = json.Value;
                    break;
                case OkResult _:
                case EmptyResult _:
                    responseObj.Data = null;
                    break;
                default:
                    responseObj.Data = result;
                    break;
            }

            context.Result = new JsonResult(responseObj);
        }       
    }
}
