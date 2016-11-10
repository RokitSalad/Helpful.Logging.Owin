using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Helpful.Logging.Owin
{
    public class LoggingContextMiddleware : OwinMiddleware
    {
        private readonly IDictionary<string, Func<string>> _requestHeaders;

        public LoggingContextMiddleware(OwinMiddleware next, IDictionary<string, Func<string>> requestHeaders) : base(next)
        {
            _requestHeaders = requestHeaders;
        }

        public override async Task Invoke(IOwinContext context)
        {
            foreach (var key in _requestHeaders.Keys)
            {
                string headerValue = context.Request.Headers[key] ?? _requestHeaders[key]();
                if(!string.IsNullOrWhiteSpace(headerValue))
                {
                    LoggingContext.Set(key, headerValue);
                }
            }
            await Next.Invoke(context);
        }
    }
}
