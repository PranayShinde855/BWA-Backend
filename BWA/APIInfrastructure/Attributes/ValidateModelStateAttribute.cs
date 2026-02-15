using BWA.ServiceEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BWA.APIInfrastructure.Attributes
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Keys.Where(k => context.ModelState[k].Errors.Count > 0).
                    Select(k =>
                    new Errors { PropertyName = k, ErrorMessages = context.ModelState[k].Errors.Select(p => p.ErrorMessage).ToArray() }).ToList();

                var responseObj = new ResponseModel
                {
                    Message = "Bad Request",
                    StatusCode = 400,
                    Errors = errors
                };

                context.Result = new JsonResult(responseObj)
                {
                    StatusCode = 400
                };
            }
        }
    }
}
