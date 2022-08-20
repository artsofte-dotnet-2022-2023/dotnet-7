using System;
using System.Collections.Generic;
using System.Linq;
using CoreLib.DependencyInjectionTest;
using CoreLib.TestType.Attributes;
using CoreLib.TestType.Enums;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace DiTask2.Test;

public class UnitTestDiTask2Test : BaseDependencyInjectionTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    
    public UnitTestDiTask2Test(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    /// <summary>
    /// Тест на проверку, инжектирование классов
    /// Инжектировать в Trnasient класс три Scope класса и в 1 из 3 Scope инжектировать Singleton
    /// все от разных интерфейсов!
    /// </summary>
    [Fact]
    [TypeTest(TestTypeEnum.External)]
    public void CheckInjectionExternal()
    {
        try
        {
            // логика теста
            var testAction = (IServiceCollection services) =>
            {
                // Здесь мы дублируем вызов того кода, что писали студенты в своем классе
                ActionForTest(services);
                var transientDescriptor = services.FirstOrDefault(value => value.Lifetime == ServiceLifetime.Transient);
                if (transientDescriptor == null)
                {
                    throw new Exception($"{ServiceLifetime.Transient} класс не существует");
                }
                var ctorParamTypeList = GetCtorParams(transientDescriptor);
                
                if (ctorParamTypeList == null || ctorParamTypeList.Count != 3)
                {
                    throw new Exception($"{ServiceLifetime.Transient} класс должен инжектировать в себя 3 класса типа {ServiceLifetime.Scoped}");
                }
                
                foreach (var ctorParamType in ctorParamTypeList)
                {
                    var injectableScopedClass = services.First(value => value.ServiceType == ctorParamType);
                    if (injectableScopedClass.Lifetime != ServiceLifetime.Scoped)
                    {
                        throw new Exception($"{ServiceLifetime.Transient} класс должен инжектировать в себя 3 класса типа {ServiceLifetime.Scoped}");
                    }
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

    /// <summary>
    /// Тест на проверку, инжектирование классов
    /// Инжектировать в Trnasient класс три Scope класса и в 1 из 3 Scope инжектировать Singleton
    /// все от разных интерфейсов!
    /// </summary>
    [Fact]
    [TypeTest(TestTypeEnum.Internal)]
    public void CheckInjectionInternal()
    {
        try
        {
            // логика теста
            var testAction = (IServiceCollection services) =>
            {
                // Здесь мы дублируем вызов того кода, что писали студенты в своем классе
                ActionForTest(services);
                var transientDescriptor = services.FirstOrDefault(value => value.Lifetime == ServiceLifetime.Transient);
                var ctorParamTypeList = GetCtorParams(transientDescriptor);
                
                var countSingletonInjection = 0;
                foreach (var ctorParamType in ctorParamTypeList)
                {
                    var injectableScopedClass = services.First(value => value.ServiceType == ctorParamType);
                    var ctorParamScopeClassTypeList = GetCtorParams(injectableScopedClass);
                    if (ctorParamScopeClassTypeList is { Count: 1 })
                    {
                        var injectableClass = services
                            .Single(value => value.ServiceType == ctorParamScopeClassTypeList.First()).Lifetime ;
                        if (injectableClass != ServiceLifetime.Singleton)
                        {
                            throw new Exception($"Один {ServiceLifetime.Scoped} класс должен инжектировать в себя 1 класс типа {ServiceLifetime.Singleton}");
                        }
                        countSingletonInjection++;
                    }
                }
                if (countSingletonInjection != 1)
                {
                    throw new Exception($"Один {ServiceLifetime.Scoped} класс должен инжектировать в себя 1 класс типа {ServiceLifetime.Singleton}");
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

    /// <summary>
    /// Параметры конструктора из класса зареганного в дескрипторе
    /// </summary>
    private List<Type>? GetCtorParams(ServiceDescriptor serviceDescriptor) 
        => serviceDescriptor.ImplementationType?
            .GetConstructors()
            .First()
            .GetParameters()
            .Select(parameter => parameter.ParameterType)
            .ToList();
}