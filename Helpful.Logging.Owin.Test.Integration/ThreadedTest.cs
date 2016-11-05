using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Helpful.Logging.Owin.Test.Integration
{
    [TestFixture]
    public class ThreadedTest
    {
        // NB: Only one of these tests can be executed successfully for any single load of the test service.
        //     This is because both tests add to the result set with contradictory data.
        [Test]
        public async Task TestWithHeader()
        {
            Parallel.For(0, 200, async i => await SendNumber(i, "testheader"));

            var result = GetLog();

            foreach (var log in result)
            {
                dynamic logEntry = JsonConvert.DeserializeObject<dynamic>(log);
                Assert.AreEqual(logEntry.testheader.ToString(), logEntry.InnerMessage.ToString());
            }
        }

        [Test]
        public async Task TestWithoutHeader()
        {
            Parallel.For(0, 200, async i => await SendNumber(i, string.Empty));

            var result = GetLog();

            foreach (var log in result)
            {
                dynamic logEntry = JsonConvert.DeserializeObject<dynamic>(log);
                Assert.AreEqual(logEntry.testheader.ToString(), "default value");
            }
        }

        private async Task SendNumber(int number, string header)
        {
            using(var client = new HttpClient())
            {
                for(int i=0;i<10;i++)
                {
                    var message = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:7999/test/{number}");
                    if(!string.IsNullOrEmpty(header))
                    {
                        message.Headers.Add(header, number.ToString());
                    }
                    await client.SendAsync(message);
                }
            }
        }

        private List<dynamic> GetLog()
        {
            using (var client = new HttpClient())
            {
                var message = new HttpRequestMessage(HttpMethod.Get, "http://localhost:7999/test/logs");
                HttpResponseMessage sendAsync = client.SendAsync(message).Result;
                var stringContent = sendAsync.Content.ReadAsStringAsync().Result;
                List<dynamic> result = JsonConvert.DeserializeObject<List<dynamic>>(stringContent);
                return result;
            }
        }
    }
}
