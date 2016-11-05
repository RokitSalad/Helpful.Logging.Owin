using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Helpful.Logging.Owin
{
    public class DebugTrafficLoggingMiddleware : OwinMiddleware
    {
        private ILogger Logger => this.GetLogger();

        public DebugTrafficLoggingMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            string content;
            using (var sr = new StreamReader(context.Request.Body))
            {
                content = sr.ReadToEnd();
            }

            var requestLog = new
            {
                Method = request.Method,
                Url = request.Uri,
                QueryString = request.QueryString.Value,
                Content = content
            };

            Logger.Debug(new {Request = requestLog});

            if (Logger.IsDebugEnabled) //use trace for logging the response
            {
                using (var captureResponseBody = new CaptureResponseBody(context))
                {
                    await Next.Invoke(context);
                    var responseBody = await captureResponseBody.GetBody();

                    Logger.Debug(new {Response = responseBody });
                }
            }
            else
            {
                await Next.Invoke(context);
            }
            
        }
    }
}
