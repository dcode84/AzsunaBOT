using AzsunaBOT.Helpers.Processes;
using AzsunaBOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers
{
    public static class ChannelHelper
    {
        private static List<ChannelModel> _channels;


        public static async Task<ulong> GetChannelId(string day)
        {
            var channels = await GetChannelDataAsync();

            var channelObject = channels.SingleOrDefault(c => c.Day == day);
            var channelId = Convert.ToUInt64(channelObject.DiscordChannel);

            return channelId;
        }

        private static async Task<List<ChannelModel>> GetChannelDataAsync()
        {
            _channels = new List<ChannelModel>();

            return await Task.Run(() =>
            {
                try { _channels = JsonDataProcessor.DeserializeChannelDataAsync("rosterChannels.json").Result; }
                catch (Exception e) { Console.WriteLine(e.Message); }

                return _channels;
            });
        }
    }
}
