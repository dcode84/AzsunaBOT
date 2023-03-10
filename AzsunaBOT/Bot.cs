using AzsunaBOT.Commands;
using AzsunaBOT.Helpers;
using AzsunaBOT.Helpers.Message;
using AzsunaBOT.Helpers.Processes;
using DataLibrary.DataAccess;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzsunaBOT
{
    public class Bot
    {
        public static DiscordClient Client { get; set; }
        private CommandsNextExtension _commands { get; set; }
        private ConfigJson _botConfig { get; set; }

        public static IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public Bot(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public async Task RunAsync()
        {
            try { _botConfig = JsonDataProcessor.DeserializeConfigDataAsync("config.json").Result; }
            catch (Exception e) { Console.WriteLine(e.Message); }

            var config = new DiscordConfiguration
            {
                Token = _botConfig.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug

            };

            Client = new DiscordClient(config);
            Client.Ready += OnClientReady;
            Client.MessageCreated += MessageListener.OnMessageSent;

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { _botConfig.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true,
                Services = _serviceProvider
            };

            _commands = Client.UseCommandsNext(commandsConfig);
            _commands.RegisterCommands<MVPCommands>();
            _commands.RegisterCommands<UserAttendanceCommands>();
            _commands.RegisterCommands<AdminAttendanceCommands>();
            _commands.RegisterCommands<UWUCommands>();

            RosterHelper.DiscordClient = Client;

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);
            _configuration = builder.Build();


            services.AddSingleton<IDataAccess, DataAccess>()
                .AddSingleton(_configuration)
                .BuildServiceProvider();
        }

    }
}
