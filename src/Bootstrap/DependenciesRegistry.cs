using System;
using Microsoft.Extensions.DependencyInjection;
using Calculator.MathServices;
using Calculator.MathServices.Helpers;

namespace Calculator.Bootstrap
{
    internal static class DependenciesRegistry
    {
        public static ServiceProvider Bootstrap()
        {
            var collection = new ServiceCollection();
            collection.AddScoped<IMathService, MathService>();
            collection.AddScoped<IInputParser, InputParser>();
            collection.AddScoped<IUserFlagParser, UserFlagParser>();

            var serviceProvider = collection.BuildServiceProvider();      
            return serviceProvider;
        }
    }
}