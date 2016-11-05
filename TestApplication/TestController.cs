using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Helpful.Logging;

namespace TestApplication
{
    public class TestController : ApiController
    {
        [Route("test/{number}")]
        public HttpResponseMessage Post(int number)
        {
            this.GetLogger().Info(number);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("test/logs")]
        public List<string> Get()
        {
            return LogStore.LogEntries;
        }

        /// <summary>
        /// REMOVE <appender-ref ref="testAppender" /> FROM APP.CONFIG TO USE THIS ENDPOINT
        /// </summary>
        /// <returns></returns>
        [Route("test/debuglog")]
        public string GetLogging()
        {
            var logger = this.GetLogger();
            logger.Info("This should be surrounded by debug logs.");
            return "This should appear in a debug log.";
        }

        /// <summary>
        /// REMOVE <appender-ref ref="testAppender" /> FROM APP.CONFIG TO USE THIS ENDPOINT
        /// </summary>
        /// <returns></returns>
        [Route("test/debuglog/complex")]
        public ResponseModel GetLoggingComplexObject()
        {
            var logger = this.GetLogger();
            logger.Info("This should be surrounded by debug logs.");
            return new ResponseModel
            {
                Value = "This should appear in a debug log.",
                DateValue = DateTime.Now
            };
        }
    }

    public class ResponseModel
    {
        public string Value { get; set; }
        public DateTime DateValue { get; set; }
    }
}
