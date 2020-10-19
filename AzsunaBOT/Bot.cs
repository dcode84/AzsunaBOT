using AzsunaBOT.Commands;
using AzsunaBOT.Helpers.Message;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AzsunaBOT
{
    public class Bot
    {
        private DiscordClient _client { get; set; }
        private CommandsNextExtension _commands { get; set; }

        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug
            };

            _client = new DiscordClient(config);

            _client.Ready += OnClientReady;
            _client.MessageCreated += MessageListener.OnMessageSent;

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true
            };

            _commands = _client.UseCommandsNext(commandsConfig);
            _commands.RegisterCommands<MVPCommands>();
            _commands.RegisterCommands<UWUCommands>();

            await _client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }

    }
}
