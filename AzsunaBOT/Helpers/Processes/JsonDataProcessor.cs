using AzsunaBOT.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers.Processes
{
    public static class JsonDataProcessor
    {
        private static string _jsonString;

        private static async Task<Task> ReadDataFromJsonAsync(string path)
        {
            _jsonString = string.Empty;

            using (var fs = File.OpenRead(path))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                _jsonString = await sr.ReadToEndAsync().ConfigureAwait(false);

            return Task.CompletedTask;
        }

        public static async Task<List<MVPData>> DeserializeMvpDataAsync(string path)
        {
            await ReadDataFromJsonAsync(path).Result;
            var mvpObject = Task.Run(() => JsonConvert.DeserializeObject<List<MVPData>>(_jsonString));

            return mvpObject.Result;
        }

        public static async Task<ConfigJson> DeserializeConfigDataAsync(string path)
        {
            await ReadDataFromJsonAsync(path);
            var configObject = Task.Run(() => JsonConvert.DeserializeObject<ConfigJson>(_jsonString));

            return configObject.Result;
        }

        public static async Task<List<ChannelModel>> DeserializeChannelDataAsync(string path)
        {
            await ReadDataFromJsonAsync(path);
            var channelObject = Task.Run(() => JsonConvert.DeserializeObject<List<ChannelModel>>(_jsonString));

            return channelObject.Result;
        }
    }
}
