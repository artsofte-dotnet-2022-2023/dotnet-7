using System;
using System.Linq;
using CoreLib.DependencyInjectionTest;
using CoreLib.TestType.Attributes;
using CoreLib.TestType.Enums;
using DiTask3.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace DiTask3.Test;

public class UnitTestDiTask3Test : BaseDependencyInjectionTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    
    public UnitTestDiTask3Test(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
   
    /// <summary>
    /// Тестирование интернал задания
    /// </summary> Action<string> dima
    [Fact]
    [TypeTest(TestTypeEnum.Internal)]
    public void Test1()
    {
        try
        {
            // логика теста
            var testAction = (IServiceCollection services) =>
            {
                // Здесь мы дублируем вызов того кода, что писали студенты в своем классе
                ActionForTest(services);
                var serviceDescriptor = services.Where(descriptor => descriptor.ServiceType == typeof(ISomeAction)).ToList();
                if (serviceDescriptor.Count != 0)
                {
                    throw new Exception($"Вы должны были удалить {nameof(ISomeAction)} сервис");
                }
                
                var threeScope = services.Where(descriptor => descriptor.ServiceType == typeof(ISanWay)).ToList();
                if (threeScope.Count != 2)
                {
                    throw new Exception($"Должен быть два {nameof(ISanWay)} сервиса");
                }

                var oneTransient = services.Where(descriptor => descriptor.ServiceType == typeof(IHelloWorld)).ToList();
                if (oneTransient.Count != 1)
                {
                    throw new Exception($"Должен быть один {nameof(IHelloWorld)} сервис");
                }
            };
        
            using var application = new DiTestTestWebApplication<Program>(testAction);
            application.CreateClient();
            Assert.True(true);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.Message);
            Assert.True(false);
        }
    }
    
    [Fact]
    [TypeTest(TestTypeEnum.External)]
    public void Test2()
    {
        try
        {
            // логика теста
            var testAction = (IServiceCollection services) =>
            {
                // Здесь мы дублируем вызов того кода, что писали студенты в своем классе
                ActionForTest(services);
                var threeScope = services.Where(descriptor => descriptor.ServiceType == typeof(ISanWay)).ToList();

                if (threeScope[0].ImplementationType == typeof(SanWay))
                {
                    throw new Exception($"Вы должны были поменять местами {nameof(ISanWay)} сервисы");
                }
                
                if (threeScope[1].ImplementationType != typeof(SanWay))
                {
                    throw new Exception($"Вы должны были поменять местами {nameof(ISanWay)} сервисы");
                }
                
                var oneTransient = services.Where(descriptor => descriptor.ServiceType == typeof(IHelloWorld)).ToList();
                if (oneTransient[0].ImplementationType == typeof(HelloWorld))
                {
                    throw new Exception($"Должен быть переопределить {nameof(HelloWorld)} сервис");
                }
            };
        
            using var application = new DiTestTestWebApplication<Program>(testAction);
            application.CreateClient();
            Assert.True(true);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.Message);
            Assert.True(false);
        }
    }
}