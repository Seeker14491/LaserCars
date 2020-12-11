using Harmony;
using Spectrum.API.Configuration;
using Spectrum.API.Interfaces.Plugins;
using Spectrum.API.Interfaces.Systems;
using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace LaserCars
{
    [UsedImplicitly]
    public class Entry : IPlugin
    {
        public void Initialize(IManager manager, string ipcIdentifier)
        {
            try
            {
                var harmony = HarmonyInstance.Create("pw.seekr.lasercars");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            InitSettings();
        }

        void InitSettings()
        {
            _settings = new Settings("CheatStatus");
            _settings.Save();
        }

        public static void SetKey(string key, object value)
        {
            var filename = G.Sys.ProfileManager_.CurrentProfile_.FileName_;

            if (!_settings.ContainsKey(filename))
                _settings[filename] = new Dictionary<string, object>();

            var data = _settings.GetItem<Dictionary<string, object>>(filename);

            data[key] = value;

            _settings[filename] = data;

            _settings.Save();
        }

        public static T GetKey<T>(string key, T defaultvalue)
        {
            var filename = G.Sys.ProfileManager_.CurrentProfile_.FileName_;

            if (!_settings.ContainsKey(filename))
                return defaultvalue;

            var data = _settings.GetItem<Dictionary<string, object>>(filename);

            return (T) data[key];
        }

        private static Settings _settings;
    }
}