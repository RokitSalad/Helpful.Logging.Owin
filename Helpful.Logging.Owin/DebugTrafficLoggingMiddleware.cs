using System.IO;
using System.Text;
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
            string content = new StreamReader(context.Request.Body).ReadToEnd();

            byte[] requestData = Encoding.UTF8.GetBytes(content);
            context.Request.Body = new MemoryStream(requestData);

            var request = context.Request;

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
