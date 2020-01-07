using System;
using Microsoft.Extensions.DependencyInjection;
using Calculator.MathServices;

namespace Calculator.Bootstrap
{
    internal static class DependenciesRegistry
    {
        public static ServiceProvider Bootstrap()
        {
            var collection = new ServiceCollection();
            collection.AddScoped<IMathService, MathService>();

            var serviceProvider = collection.BuildServiceProvider();      
            return serviceProvider;
        }
    }
}