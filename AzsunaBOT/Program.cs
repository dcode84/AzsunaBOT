using DataLibrary.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzsunaBOT
{
    class Program
    {
        public static IConfiguration _configuration;


        static void Main(string[] args)
        {
            MainAsync(args);
        }

        static async void MainAsync(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", true, true);
            _configuration = builder.Build();
            ConfigureServices(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var bot = new Bot(_configuration, serviceProvider);
            bot.RunAsync().GetAwaiter().GetResult();

            await Task.Delay(-1);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDataAccess, DataAccess>();
            services.AddSingleton<IConfiguration>(_configuration);
        }

    }
}
