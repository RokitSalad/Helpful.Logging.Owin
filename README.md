# Helpful.Logging.Owin
log4net based logging library which automatically wraps log entries with correlation ids and other tracing info

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

```c#
this.GetLogger().Info("a leg entry", new Exception());
```
The object.GetLogger() extension method can be used anywhere to get a log4net logger. This logger can be used in the usual way. Any log messages will be wrapped in an object listing all tracing info.

```c#
LoggingContext.Set("some-key", "some value");
```
The LoggingContext is static and can be used from anywhere in your code if you want to add new items or access existing items. For example, if you're consuming an event which also contained a correlation id, you could add this directly to the LoggingContext. The LoggingContext values will persist as you move from thread to thread so you should be safe with async code.

##Install
Install-Package Helpful.Logging.Owin

##Note
This is tightly coupled to log4net and Newtonsoft.Json - if you don't use them, this is no use to you.