using System;
using System.Collections.Generic;
using System.Web.Http;
using Helpful.Logging.Owin;
using Owin;

namespace TestApplication
{
    public class Config
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            appBuilder.Use<LoggingContextMiddleware>(new Dictionary<string, Func<string>> { { "testheader", () => "default value"}})
                .Use<DebugTrafficLoggingMiddleware>()
                .UseWebApi(config);
        }
    }
}