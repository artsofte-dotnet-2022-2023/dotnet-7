using System;
using System.Linq;
using CoreLib.DependencyInjectionTest;
using CoreLib.TestType.Attributes;
using CoreLib.TestType.Enums;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace DiTask1.Test;

public class UnitTestDiTask1Test : BaseDependencyInjectionTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTestDiTask1Test(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    /// <summary>
    /// Тест на проверку, что в системе зарегстрировано
    /// 1 - singleton
    /// 3 - scope
    /// 1 - transient
    /// все от разных интерфейсов!
    /// </summary>
    [Fact]
    [TypeTest(TestTypeEnum.External)]
    public void CheckRegistrationType()
    {
        try
        {
            // логика теста
            var testAction = (IServiceCollection services) =>
            {
                // Здесь мы дублируем вызов того кода, что писали студенты в своем классе
                ActionForTest(services);
                var oneSingleton = services.Where(descriptor => descriptor.Lifetime == ServiceLifetime.Singleton).ToList();
                if (oneSingleton.Count != 1)
                {
                    throw new Exception($"Должен быть 1 {ServiceLifetime.Singleton} сервис");
                }
                var threeScope = services.Where(descriptor => descriptor.Lifetime == ServiceLifetime.Scoped).ToList();
                if (threeScope.Count != 3)
                {
                    throw new Exception($"Должено быть 3 {ServiceLifetime.Scoped} сервиса");
                }
                var oneTransient = services.Where(descriptor => descriptor.Lifetime == ServiceLifetime.Transient).ToList();
                if (oneTransient.Count != 1)
                {
                    throw new Exception($"Должен быть 1 {ServiceLifetime.Transient} сервис");
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