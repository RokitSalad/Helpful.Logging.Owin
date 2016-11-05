using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Topshelf;

namespace TestApplication
{
    public class Service
    {
        private IDisposable _disposable;

        public bool Start(HostControl hostControl)
        {
            _disposable = WebApp.Start<Config>("http://*:7999");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _disposable.Dispose();
            return true;
        }
    }
}
