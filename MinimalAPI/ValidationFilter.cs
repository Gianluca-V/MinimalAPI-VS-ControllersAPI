
using FluentValidation;
using MinimalAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace MinimalAPI
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext Context, EndpointFilterDelegate Next)
        {
            var validator = Context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if (validator is not null)
            {
                var entity = Context.Arguments.OfType<T>().FirstOrDefault(a => a?.GetType() == typeof(T));

                if (entity is not null)
                {
                    var validation = await validator.ValidateAsync(entity);

                    if (!validation.IsValid)
                    {
                        return Results.ValidationProblem(validation.ToDictionary());
                    }
                    return await Next(Context);
                }
                else
                {
                    return Results.Problem("Could not find type to validate");
                }
            }
            return await Next(Context);
        }
    }
}
