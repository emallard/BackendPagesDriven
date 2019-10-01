using System.Threading.Tasks;
using CocoriCore;
using Microsoft.AspNetCore.Http;

namespace CocoriCore
{
    public class HttpResponseWriterHtmlHandler : IHttpReponseWriterHandler
    {

        public async Task WriteResponseAsync(HttpResponseWriterContext context)
        {
            var response = (HtmlResponse)context.Response;
            context.HttpResponse.ContentType = "text/html";
            await context.HttpResponse.WriteAsync(response.Html);

        }
    }
}