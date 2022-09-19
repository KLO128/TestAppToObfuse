using TestAppToObfuse.Services.Impl;

var service = new SomeService(new DependencyService(), false);

Console.WriteLine("Hello World!");

Console.WriteLine($"The same: {service.DoIt("Exercise 1")}");
Console.WriteLine($"Previous Result: {service.PreviousResult}");
