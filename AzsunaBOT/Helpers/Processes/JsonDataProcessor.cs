﻿using AzsunaBOT.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
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
    }
}
