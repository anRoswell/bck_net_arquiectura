using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Infrastructure.Filters
{
    public class ResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!(context.Result is EmptyResult))
            {
                await next();
            }
            else
            {
                //context.Cancel = true;
            }
        }
    }
}
