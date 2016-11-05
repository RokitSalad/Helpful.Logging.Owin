using log4net.Config;
using Topshelf;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            HostFactory.Run
                    (
                        c =>
                        {
                            c.Service<Service>
                                (
                                    sc =>
                                    {
                                        sc.ConstructUsing(svc => new Service());
                                        sc.WhenStarted(
                                            (service, hostControl) => service.Start(hostControl));
                                        sc.WhenStopped((service, hostControl) => service.Stop(hostControl));
                                    }
                                );

                            c.SetServiceName("test service");
                            c.SetDisplayName("test service");
                            c.SetDescription("test service");
                            c.EnableShutdown();
                            c.StartAutomatically();

                            c.RunAsLocalSystem();
                        }
                    );
        }
    }
}
