using CocoriCore;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Comptes.Api
{
    public static class HttpResponseWriterConfiguration
    {
        public static HttpResponseWriterOptions Options()
        {
            var builder = new HttpResponseWriterOptionsBuilder();
            builder.For<FileResponse>().Call<HttpResponseWriterFileHandler>();
            //builder.For<IODataResponse>().Call<HttpResponseWriterODataHandler>();
            builder.For<HtmlResponse>().Call<HttpResponseWriterHtmlHandler>();
            builder.For<SvgResponse>().Call<HttpResponseWriterSvgHandler>();
            builder.For<IClaimsResponse>().Call<HttpResponseWriterClaimsHandler>();
            builder.For<object>().Call<HttpResponseWriterDefaultHandler>();
            return builder.Options;
        }
    }
}
