using System;
using Microsoft.Extensions.DependencyInjection;
using Calculator.Bootstrap;
using Calculator.MathServices;

namespace Calculator
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        public static void Main()
        {
            //Handles terminating applicaiton when Ctrl + C is detected
            Console.CancelKeyPress += new ConsoleCancelEventHandler(ExitHandler);

            _serviceProvider = DependenciesRegistry.Bootstrap();
            var mathService = _serviceProvider.GetService<IMathService>();

            while (true)
            {
                var userInput = Console.ReadLine();
                Console.WriteLine($"{mathService.Add(userInput)}");
            }
        }

        protected static void ExitHandler(object sender, ConsoleCancelEventArgs args)
        {
            if(_serviceProvider == null)
            {
                return;
            }

            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
            Environment.Exit(-1);
        }
    }
}
