using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Helpful.Logging.Owin
{
    internal class CaptureResponseBody : IDisposable
    {
        private readonly Stream _stream;
        private readonly MemoryStream _buffer;

        public CaptureResponseBody(IOwinContext context)
        {
            _stream = context.Response.Body;
            _buffer = new MemoryStream();
            context.Response.Body = _buffer;
        }

        public async Task<string> GetBody()
        {
            _buffer.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(_buffer);
            return await reader.ReadToEndAsync();
        }

        public async void Dispose()
        {
            await GetBody();
            _buffer.Seek(0, SeekOrigin.Begin);
            await _buffer.CopyToAsync(_stream);
        }
    }
}