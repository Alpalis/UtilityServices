﻿using Alpalis.UtilityServices.API;
using Alpalis.UtilityServices.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoreLinq;
using OpenMod.API.Ioc;
using OpenMod.API.Plugins;
using OpenMod.API.Prioritization;
using OpenMod.Unturned.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.Services
{
    [ServiceImplementation(Lifetime = ServiceLifetime.Singleton, Priority = Priority.Normal)]
    public class ConfigurationManager : IConfigurationManager
    {
        #region Member Variables
        private readonly ILogger<ConfigurationManager> m_Logger;
        private readonly IPluginActivator m_PluginActivator;
        #endregion Member Variables

        #region Class Constructor
        public ConfigurationManager(
            ILogger<ConfigurationManager> logger,
            IPluginActivator pluginActivator)
        {
            m_Logger = logger;
            m_PluginActivator = pluginActivator;
            Configs = new();
        }
        #endregion Class Constructor

        private Dictionary<string, MainConfig> Configs { get; set; }

        public async Task LoadConfig(OpenModUnturnedPlugin plugin, MainConfig config)
        {
            Type type = config.GetType();
            foreach (PropertyInfo property in type.GetProperties())
            {
                try
                {
                    property.SetValue(config, plugin.Configuration.GetSection(property.Name).Get(property.PropertyType));
                }
                catch (Exception)
                {
                    m_Logger.LogDebug(string.Format("The config variable \"{0}\" has been reset to the default \"{1}\".",
                        property.Name, property.GetValue(config)));
                }
            }
            if (Configs.ContainsKey(plugin.OpenModComponentId))
                Configs[plugin.OpenModComponentId] = config;
            else
                Configs.Add(plugin.OpenModComponentId, config);
            m_Logger.LogDebug(string.Format("\"{0}\" config loaded successfully!", plugin.DisplayName));
        }

        public async Task<bool> ReloadConfig(string pluginName)
        {
            OpenModUnturnedPlugin plugin = (OpenModUnturnedPlugin)m_PluginActivator.ActivatedPlugins
                .FirstOrDefault(name => name.DisplayName.Equals(pluginName, StringComparison.OrdinalIgnoreCase));
            if (plugin == null) return false;
            MainConfig? config = Configs.FirstOrDefault(obj => obj.Key == plugin.OpenModComponentId).Value;
            if (config == null) return false;
            await LoadConfig(plugin, config);
            return true;
        }

        public async Task<List<string>> ReloadAllConfig()
        {
            m_Logger.LogDebug("Reloading all plugins' configs!");
            List<string> plugins = new();
            Dictionary<string, MainConfig> configsClone = new(Configs);
            foreach (KeyValuePair<string, MainConfig> data in configsClone)
            {
                OpenModUnturnedPlugin plugin = (OpenModUnturnedPlugin)m_PluginActivator.ActivatedPlugins
                    .FirstOrDefault(name => name.OpenModComponentId.Equals(data.Key, StringComparison.Ordinal));
                if (plugin == null) continue;
                plugins.Add(plugin.DisplayName);
                m_Logger.LogInformation(string.Format("Reloading \"{0}\" config!", plugin.DisplayName));
                LoadConfig(plugin, data.Value);
            }
            m_Logger.LogDebug("Succesfully reloaded configs!");
            return plugins;
        }

        public T GetConfig<T>(OpenModUnturnedPlugin plugin)
        {
            return (T)(object)Configs[plugin.OpenModComponentId];
        }
    }
}
