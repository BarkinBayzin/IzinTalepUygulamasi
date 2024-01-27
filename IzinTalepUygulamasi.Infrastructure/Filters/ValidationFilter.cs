using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidationFilter : IAsyncActionFilter //Action'a gelen isteklerde çalışacak bir filter yazıyoruz
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) //Her filterlemede çalışacak, next ile sonraki delegeye geçişini sağlar.
    {
        if(!context.ModelState.IsValid)
        {
            KeyValuePair<string, IEnumerable<string>>[]? errors = context.ModelState
                .Where(x => x.Value.Errors.Any())
                .ToDictionary(e => e.Key, e => e.Value.Errors.Select(e => e.ErrorMessage))
                .ToArray();
            //Key bana ilgili prop'u teslim edecek, bir veya daha fazla hata mesajı gelirse de hatayı göstebileceğimiz seviyeye getirdik.
            context.Result = new BadRequestObjectResult(errors);
            return;
        }

        await next(); //Sonraki delege geçişi için next ile tamamlıyoruz.

        //Şimdi servisimi container'a ekliyoruz.
    }
}

