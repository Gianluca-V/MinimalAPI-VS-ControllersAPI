
using FluentValidation;
using FullRESTAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace MinimalAPI
{
    public class ValidationFilter<T> : IActionFilter
    {

        public void OnActionExecuted(ActionExecutedContext Context)
        {    
        }

        public async void OnActionExecuting(ActionExecutingContext Context)
        {
            var validator = Context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if (validator is not null)
            {
                var entity = Context.ActionArguments.OfType<T>().FirstOrDefault(a => a?.GetType() == typeof(T));

                if (entity is not null)
                {
                    var validation = await validator.ValidateAsync(entity);

                    if (!validation.IsValid)
                    {
                        Context.Result = new BadRequestObjectResult(validation.ToDictionary());
                        return;
                    }
                }
                else
                {
                    Context.Result = new BadRequestObjectResult("Object is null.");
                    return;
                }
            }
        }
    }
}
