﻿using System;
using System.IO;

namespace Server.Data
{
    [Serializable]
    public class ServerConfig
    {
        public string dataPath;
    }

    public static class ConfigManager
    {
        public static ServerConfig Config { get; private set; }
        public static void LoadConfig()
        {
            string text = File.ReadAllText("config.json");
            Config = Newtonsoft.Json.JsonConvert.DeserializeObject<ServerConfig>(text);
        }
    }
}
