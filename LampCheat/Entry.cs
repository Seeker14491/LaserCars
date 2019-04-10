using Harmony;
using Spectrum.API.Configuration;
using Spectrum.API.Interfaces.Plugins;
using Spectrum.API.Interfaces.Systems;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LampCheat
{
    public class Entry : IPlugin, IUpdatable
    {
        public void Initialize(IManager manager, string ipcIdentifier)
        {
            try
            {
                HarmonyInstance Harmony = HarmonyInstance.Create("com.REHERC.LampCheat");
                Harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception Greg)
            {
                Console.WriteLine(Greg.Message);
            }

            InitSettings();
        }

        void InitSettings()
        {
            settings = new Settings("CheatStatus");
            settings.Save();
        }

        public static void SetKey(string key, object value)
        {
            string filename = G.Sys.ProfileManager_.CurrentProfile_.FileName_;

            if (!settings.ContainsKey(filename))
                settings[filename] = new Dictionary<string, object>();

            Dictionary<string, object> data = settings.GetItem<Dictionary<string, object>>(filename);

            data[key] = value;

            settings[filename] = data;

            settings.Save();
        }

        public static Type GetKey<Type>(string key, Type defaultvalue)
        {
            string filename = G.Sys.ProfileManager_.CurrentProfile_.FileName_;

            if (!settings.ContainsKey(filename))
                return defaultvalue;

            Dictionary<string, object> data = settings.GetItem<Dictionary<string, object>>(filename);

            return (Type)data[key];
        }

        public void Update()
        {
            if (lamp != null)
                lamp.SetActive(Entry.GetKey<bool>("lamp.cheat", false));
        }

        static Settings settings = null;  

        GameObject lamp;
    }
}
