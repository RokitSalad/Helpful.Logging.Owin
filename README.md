# Helpful.Logging.Owin
This is Owin middleware for the Helpful.Logging logging context which allows you to automatically populate the logging context from http headers of received calls.

##Usage
```c#
public void Configuration(IAppBuilder appBuilder)
{
    var config = new HttpConfiguration();

    config.MapHttpAttributeRoutes();

    appBuilder.Use<LoggingContextMiddleware>(new Dictionary<string, Func<string>> { { "correlation-id", () => Guid.NewGuid().ToString()}});
    appBuilder.UseWebApi(config);
}
```
The above configuration will look for an HTTP header named correlation-id and add the value of that header to every log entry made as a result of that request. If the header is not found or it is empty, it will use a new guid as the value.

##Install
Install-Package Helpful.Logging.Owin

##Note
This is tightly coupled to log4net - if you don't use this, it's no use to you.