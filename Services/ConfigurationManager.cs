﻿using Alpalis.UtilityServices.API;
using Alpalis.UtilityServices.API.Events;
using Alpalis.UtilityServices.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using MoreLinq;
using OpenMod.API.Eventing;
using OpenMod.API.Ioc;
using OpenMod.API.Plugins;
using OpenMod.API.Prioritization;
using OpenMod.Unturned.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.Services
{
    [ServiceImplementation(Lifetime = ServiceLifetime.Singleton, Priority = Priority.Highest)]
    public class ConfigurationManager(
        ILogger<ConfigurationManager> logger,
        IPluginAccessor<Main> pluginAccessor,
        IEventBus eventBus,
        IPluginActivator pluginActivator) : API.IConfigurationManager
    {
        private readonly ILogger<ConfigurationManager> m_Logger = logger;
        private readonly IPluginActivator m_PluginActivator = pluginActivator;
        private readonly IPluginAccessor<Main> m_PluginAccessor = pluginAccessor;
        private readonly IEventBus m_EventBus = eventBus;

        private Dictionary<string, StoredConfig> Configs { get; set; } = [];

        #region LoadConfig from OpenModUnturnedPlugin
        public T LoadConfig<T>(OpenModUnturnedPlugin plugin)
        {
            // Creating instance of config model
            T configInstance = Activator.CreateInstance<T>();
            return LoadConfig(configInstance, plugin);
        }

        public T LoadConfig<T>(T baseConfig, OpenModUnturnedPlugin plugin)
        {
            if (baseConfig is not MainConfig)
                throw new Exception("Wrong type of config model!");
            // Casting generic type to abstract class MainConfig
            MainConfig config = (MainConfig)(object)baseConfig;
            return LoadConfigInternal<T>(config, plugin);
        }

        public async Task<T> LoadConfigAsync<T>(OpenModUnturnedPlugin plugin)
        {
            // Creating instance of config model
            T configInstance = Activator.CreateInstance<T>();
            return await LoadConfigAsync(configInstance, plugin);
        }

        public async Task<T> LoadConfigAsync<T>(T baseConfig, OpenModUnturnedPlugin plugin)
        {
            if (baseConfig is not MainConfig)
                throw new Exception("Wrong type of config model!");
            // Casting generic type to abstract class MainConfig
            MainConfig config = (MainConfig)(object)baseConfig;
            return await LoadConfigInternalAsync<T>(config, plugin);
        }
        #endregion LoadConfig from OpenModUnturnedPlugin

        #region LoadConfig Internal
        private T LoadConfigInternal<T>(MainConfig config, OpenModUnturnedPlugin plugin)
        {
            Type type = config.GetType();
            try
            {
                plugin.Configuration.Bind(config);
            }
            catch (Exception)
            {
                m_Logger.LogInformation(string.Format("Problem occured on loading config of \"{0}\" plugin!", plugin.DisplayName));
            }
            if (!config.IsValid(out List<string> errors))
            {
                foreach (string error in errors)
                    m_Logger.LogWarning(error);
            }

            // Adding/updating config to dictionary Configs
            if (Configs.ContainsKey(plugin.OpenModComponentId))
            {
                Configs[plugin.OpenModComponentId].Type = type;
                Configs[plugin.OpenModComponentId].Config = config;
            }
            else
            {
                StoredConfig newStoredConfig = new(config, type,
                    ChangeToken.OnChange(() => plugin.Configuration.GetReloadToken(), () =>
                    {
                        if (!GetConfig<Config>(m_PluginAccessor.Instance!).AutomaticConfigReload) return;
                        ReloadConfig(plugin, type);
                    }));
                Configs.Add(plugin.OpenModComponentId, newStoredConfig);
            }
            m_Logger.LogInformation(string.Format("\"{0}\" config loaded successfully!", plugin.DisplayName));
            return (T)(object)config;
        }

        private Task<T> LoadConfigInternalAsync<T>(MainConfig config, OpenModUnturnedPlugin plugin)
        {
            Type type = config.GetType();
            try
            {
                plugin.Configuration.Bind(config);
            }
            catch (Exception)
            {
                m_Logger.LogWarning(string.Format("Problem occured on loading config of \"{0}\" plugin!", plugin.DisplayName));
            }
            if (!config.IsValid(out List<string> errors))
            {
                foreach (string error in errors)
                    m_Logger.LogWarning(error);
            }
            // Adding/updating config to dictionary Configs
            if (Configs.ContainsKey(plugin.OpenModComponentId))
            {
                Configs[plugin.OpenModComponentId].Type = type;
                Configs[plugin.OpenModComponentId].Config = config;
            }
            else
            {
                StoredConfig newStoredConfig = new(config, type,
                    ChangeToken.OnChange(() => plugin.Configuration.GetReloadToken(), async () =>
                    {
                        if (!GetConfig<Config>(m_PluginAccessor.Instance!).AutomaticConfigReload) return;
                        await ReloadConfigAsync(plugin, type);
                    }));
                Configs.Add(plugin.OpenModComponentId, newStoredConfig);
            }
            m_Logger.LogInformation(string.Format("\"{0}\" config loaded successfully!", plugin.DisplayName));
            return Task.FromResult((T)(object)config);
        }
        #endregion LoadConfig Internal

        #region ReloadConfig
        public bool ReloadConfig(string pluginName)
        {
            OpenModUnturnedPlugin? plugin = m_PluginActivator.GetPluginBySimmilarName(pluginName);
            if (plugin == null) return false;
            StoredConfig? config = Configs.FirstOrDefault(obj => obj.Key == plugin.OpenModComponentId).Value;
            if (config == null) return false;
            config.Config = (MainConfig)LoadConfig(Activator.CreateInstance(config.Type), plugin);
            ConfigReloadedEvent @event = new(plugin, config.Config);
            m_EventBus.EmitAsync(m_PluginAccessor.Instance!, this, @event);
            return true;
        }

        public async Task<bool> ReloadConfigAsync(string pluginName)
        {
            OpenModUnturnedPlugin? plugin = m_PluginActivator.GetPluginBySimmilarName(pluginName);
            if (plugin == null) return false;
            StoredConfig? config = Configs.FirstOrDefault(obj => obj.Key == plugin.OpenModComponentId).Value;
            if (config == null) return false;
            config.Config = (MainConfig)await LoadConfigAsync(Activator.CreateInstance(config.Type), plugin);
            ConfigReloadedEvent @event = new(plugin, config.Config);
            await m_EventBus.EmitAsync(m_PluginAccessor.Instance!, this, @event).ConfigureAwait(false);
            return true;
        }

        private MainConfig ReloadConfig(OpenModUnturnedPlugin plugin, Type configType)
        {
            MainConfig config = (MainConfig)LoadConfig(Activator.CreateInstance(configType), plugin);
            ConfigReloadedEvent @event = new(plugin, config);
            m_EventBus.EmitAsync(m_PluginAccessor.Instance!, this, @event);
            return config;
        }

        private async Task<MainConfig> ReloadConfigAsync(OpenModUnturnedPlugin plugin, Type configType)
        {
            MainConfig config = (MainConfig)await LoadConfigAsync(Activator.CreateInstance(configType), plugin);
            ConfigReloadedEvent @event = new(plugin, config);
            await m_EventBus.EmitAsync(m_PluginAccessor.Instance!, this, @event).ConfigureAwait(false);
            return config;
        }
        #endregion ReloadConfig

        #region ReloadAllConfigs
        public List<string> ReloadAllConfigs()
        {
            m_Logger.LogDebug("Reloading all plugins' configs!");
            List<string> plugins = [];
            Dictionary<string, StoredConfig> configsClone = new(Configs);
            foreach (KeyValuePair<string, StoredConfig> data in configsClone)
            {
                OpenModUnturnedPlugin? plugin = m_PluginActivator.GetPluginById(data.Key);
                if (plugin == null) continue;
                plugins.Add(plugin.DisplayName);
                m_Logger.LogInformation(string.Format("Reloading \"{0}\" config!", plugin.DisplayName));
                ReloadConfig(plugin, data.Value.Type);
            }
            m_Logger.LogDebug("Succesfully reloaded configs!");
            return plugins;
        }

        public async Task<List<string>> ReloadAllConfigsAsync()
        {
            m_Logger.LogDebug("Reloading all plugins' configs!");
            List<string> plugins = [];
            Dictionary<string, StoredConfig> configsClone = new(Configs);
            List<Task<MainConfig>> tasks = [];
            foreach (KeyValuePair<string, StoredConfig> data in configsClone)
            {
                OpenModUnturnedPlugin? plugin = m_PluginActivator.GetPluginById(data.Key);
                if (plugin == null) continue;
                plugins.Add(plugin.DisplayName);
                m_Logger.LogInformation(string.Format("Reloading \"{0}\" config!", plugin.DisplayName));
                tasks.Add(ReloadConfigAsync(plugin, data.Value.Type));
            }
            await Task.WhenAll(tasks);
            m_Logger.LogDebug("Succesfully reloaded configs!");
            return plugins;
        }
        #endregion ReloadAllConfigs

        public T GetConfig<T>(OpenModUnturnedPlugin plugin)
        {
            return (T)(object)Configs[plugin.OpenModComponentId].Config;
        }

        public List<KeyValuePair<string, string>>? GetConfigProperties(OpenModUnturnedPlugin plugin)
        {
            return Configs[plugin.OpenModComponentId].Config.GetPropertiesInString();
        }

        public List<KeyValuePair<string, string>>? GetConfigProperties(string pluginName)
        {
            OpenModUnturnedPlugin? plugin = m_PluginActivator.GetPluginBySimmilarName(pluginName);
            return plugin == null ? null : Configs[plugin.OpenModComponentId].Config.GetPropertiesInString();
        }
        public Dictionary<OpenModUnturnedPlugin, List<KeyValuePair<string, string>>> GetConfigProperties()
        {
            Dictionary<OpenModUnturnedPlugin, List<KeyValuePair<string, string>>> configs = [];
            Dictionary<string, StoredConfig> configsClone = new(Configs);
            foreach (KeyValuePair<string, StoredConfig> data in configsClone)
            {
                OpenModUnturnedPlugin? plugin = m_PluginActivator.GetPluginById(data.Key);
                if (plugin == null) continue;
                configs.Add(plugin, Configs[plugin.OpenModComponentId].Config.GetPropertiesInString());
            }
            return configs;
        }
    }
}
