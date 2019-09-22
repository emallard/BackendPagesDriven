
using CocoriCore;
using Microsoft.AspNetCore.Hosting;
using Ninject.Extensions.NamedScope;
using Ninject.Modules;
using System;
using System.Reflection;

namespace Comptes.Api
{

    public class HttpErrorWriterConfiguration
    {

        public static HttpErrorWriterOptions Options()
        {
            var builder = new HttpErrorWriterOptionsBuilder();

            builder.SetDebugMode(() => true);
            //builder.For<DeserializeMessageException>().Call<DeserializationExceptionWriter>();
            builder.For<RouteNotFoundException>().Call<RouteNotFoundExceptionWriter>();
            builder.For<AuthenticationException>().Call<AuthenticationExceptionWriter>();
            builder.For<InvalidTokenException>().Call<InvalidTokenExceptionWriter>();
            builder.For<Exception>().Call<DefaultExceptionWriter>();

            return builder.Options;
        }
    }
}
