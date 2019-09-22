using System.IO;
using System.Threading.Tasks;
using CocoriCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Comptes.Api
{
    public class HttpResponseWriterClaimsHandler : IHttpReponseWriterHandler
    {
        private readonly JsonSerializer jsonSerializer;

        public HttpResponseWriterClaimsHandler(JsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        public async Task WriteResponseAsync(HttpResponseWriterContext context)
        {
            var response = (IClaimsResponse)context.Response;
            string claimsString;
            string responseString;
            context.HttpResponse.ContentType = "application/json; charset=utf-8";
            using (var stringWriter = new StringWriter())
            {
                jsonSerializer.Serialize(stringWriter, response.GetClaims());
                claimsString = stringWriter.ToString();
            }
            using (var stringWriter = new StringWriter())
            {
                jsonSerializer.Serialize(stringWriter, response.GetResponse());
                responseString = stringWriter.ToString();
            }

            context.HttpResponse.Cookies.Append("X-Auth", claimsString);
            await context.HttpResponse.WriteAsync(responseString);
        }
    }
}