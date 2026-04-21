using LoggingProxy;
using LoggingProxy.Interfaces;

var logger = new ConsoleLogger();
var proxy = new LoggingProxy<ICalculator>(logger)
                .CreateInstance(new Calculator());

proxy.Add(3, 5);