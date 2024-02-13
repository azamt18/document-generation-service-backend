using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace API.Controllers.Filters;

public class DefaultRequestLog : ActionFilterAttribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var log = Logger.DefaultLog;
        var actionExecutedContext = context.HttpContext;

        try
        {
            var logLine = $"{context.Controller}\t{context.HttpContext.Response.StatusCode}\t{context.HttpContext.Request.Method}";
            log.Info(logLine);

            if (context.HttpContext.Request.Method == HttpMethod.Post.ToString() || context.HttpContext.Request.Method == HttpMethod.Put.ToString())
            {
                try
                {
                    using (var stream = new MemoryStream())
                    {
                        // make sure that body is read from the beginning
                        actionExecutedContext.Request.Body.Seek(0, SeekOrigin.Begin);
                        await actionExecutedContext.Request.Body.CopyToAsync(stream);
                        log.Info(Encoding.UTF8.GetString(stream.ToArray()));
                    
                        // this is required, otherwise model binding will return null
                        actionExecutedContext.Request.Body.Seek(0, SeekOrigin.Begin);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            
            await next();
        }
        catch (Exception e)
        {
            log.Error(e.Message);
        }
    }
}