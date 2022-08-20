using DiTask3.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder();
builder.Services.AddTransient<ISomeAction, SomeAction>();
builder.Services.AddTransient<ISanWay, SanWay>();
builder.Services.AddTransient<IHelloWorld, HelloWorld>();

ChangeDiLogicExtension3.DoTaskHereAction(builder.Services);

var app = builder.Build();

app.Run();

public static class ChangeDiLogicExtension3
{
    public class Dima : ISanWay
    {

    }

    public class HelloWorldTwo : IHelloWorld
    {

    }

    // write your code  here !
    public static void DoTaskHereAction(IServiceCollection serviceCollection)
    {
        // // 1 задание
        // var serviceDescriptor = serviceCollection.First(descriptor => descriptor.ServiceType == typeof(ISomeAction));
        // serviceCollection.Remove(serviceDescriptor);
        //
        // // 2 задание
        // static void Swap(IServiceCollection list, int indexA, int indexB)
        // {
        //     (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        // }
        //
        // var serviceDescriptorTaskTwo = serviceCollection.First(descriptor => descriptor.ServiceType == typeof(ISanWay));
        // serviceCollection.AddTransient<ISanWay, Dima>();
        // Swap(serviceCollection, serviceCollection.IndexOf(serviceDescriptorTaskTwo), serviceCollection.Count - 1);
        //
        // // задание 3
        // var newHelloWorld = new ServiceDescriptor(typeof(IHelloWorld), typeof(HelloWorldTwo), ServiceLifetime.Transient);
        // serviceCollection.Replace(newHelloWorld);
    }
}

public partial class Program { }