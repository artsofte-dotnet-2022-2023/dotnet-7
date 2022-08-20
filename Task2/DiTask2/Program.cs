using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder();
ChangeDiLogicExtension2.DoTaskHereAction(builder.Services);
var app = builder.Build();

app.Run();

public static class ChangeDiLogicExtension2
{
    // write your code  here !
    public static void DoTaskHereAction(IServiceCollection serviceCollection)
    {
        // and here !
    }
}

public partial class Program { }