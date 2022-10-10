using Alpalis.UtilityServices.API;
using Alpalis.UtilityServices.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoreLinq;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;
using OpenMod.API;
using OpenMod.API.Ioc;
using OpenMod.API.Plugins;
using OpenMod.API.Prioritization;
using OpenMod.Core.Plugins;
using OpenMod.Unturned.Plugins;
using SDG.Unturned;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Alpalis.UtilityServices.Services
{
    [ServiceImplementation(Lifetime = ServiceLifetime.Singleton, Priority = Priority.Normal)]
    public class ConfigurationManager : IConfigurationManager
    {
        #region Member Variables
        private readonly ILogger<ConfigurationManager> m_Logger;
        private readonly IPluginActivator m_PluginActivator;
        private readonly IRuntime m_Runtime;
        #endregion Member Variables

        #region Class Constructor
        public ConfigurationManager(
            ILogger<ConfigurationManager> logger,
            IRuntime runtime,
            IPluginActivator pluginActivator)
        {
            m_Logger = logger;
            m_PluginActivator = pluginActivator;
            Configs = new();
            m_Runtime = runtime;
        }
        #endregion Class Constructor

        private Dictionary<string, MainConfig> Configs { get; set; }

        private OpenModUnturnedPlugin GetPluginByName(string pluginName) => (OpenModUnturnedPlugin) m_PluginActivator.ActivatedPlugins
                .FirstOrDefault(name => name.DisplayName.Equals(pluginName, StringComparison.OrdinalIgnoreCase));

        #region LoadConfig from Assembly
        public T LoadConfig<T>()
        {
            // Creating instance of config model
            T configInstance = Activator.CreateInstance<T>();
            //Getting asembly of calling plugin
            Assembly plugin = Assembly.GetCallingAssembly();
            return LoadConfig(configInstance, plugin);
        }

        public T LoadConfig<T>(T configInstance)
        {
            //Getting asembly of calling plugin
            Assembly plugin = Assembly.GetCallingAssembly();
            return LoadConfig(configInstance, plugin);
        }

        private T LoadConfig<T>(T configInstance, Assembly? plugin)
        {
            if (plugin == null)
                throw new Exception("Could not plugin assembly!");
            //Getting basic plugin data by finding assembly attribute located in the main class
            var metadata = plugin.GetCustomAttribute<PluginMetadataAttribute>();
            if (metadata == null)
                throw new Exception("Could not find plugin metadata!");
            // If model class is not extending MainConfig abstract class we need to throw an exception
            if (configInstance is not MainConfig)
                throw new Exception("Wrong type of config model!");
            MainConfig config = (MainConfig)(object)configInstance;
            //Creating new config file
            var configFile = new ConfigurationBuilder()
                .SetBasePath(PluginHelper.GetWorkingDirectory(m_Runtime, metadata.Id))
                .AddYamlFile("config.yaml", optional: false)
                .Build();
            //Building new service provider to access data in plugin's config file
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configFile);
            serviceCollection.AddSingleton<IConfiguration>(configFile);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return LoadConfigInternal<T>(config, configuration, metadata.Id, metadata.DisplayName!);
        }
        #endregion LoadConfig from Assembly

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
            return LoadConfigInternal<T>(config, plugin.Configuration, plugin.OpenModComponentId, plugin.DisplayName);
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
            return await LoadConfigInternalAsync<T>(config, plugin.Configuration, plugin.OpenModComponentId, plugin.DisplayName);
        }
        #endregion LoadConfig from OpenModUnturnedPlugin

        #region LoadConfig Internal
        private T LoadConfigInternal<T>(MainConfig config, IConfiguration configuration, string pluginId, string pluginDisplayName)
        {
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                try
                {
                    property.SetValue(config, configuration.GetSection(property.Name).Get(property.PropertyType));
                }
                catch (Exception)
                {
                    m_Logger.LogDebug(string.Format("The config variable \"{0}\" has been reset to the default \"{1}\".",
                        property.Name, property.GetValue(config)));
                }
            }
            // Adding/updating config to dictionary Configs
            if (Configs.ContainsKey(pluginId)) Configs[pluginId] = config;
            else Configs.Add(pluginId, config);
            m_Logger.LogDebug(string.Format("\"{0}\" config loaded successfully!", pluginDisplayName));
            return (T)(object)config;
        }

        private async Task<T> LoadConfigInternalAsync<T>(MainConfig config, IConfiguration configuration, string pluginId, string pluginDisplayName)
        {
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                try
                {
                    property.SetValue(config, configuration.GetSection(property.Name).Get(property.PropertyType));
                }
                catch (Exception)
                {
                    m_Logger.LogDebug(string.Format("The config variable \"{0}\" has been reset to the default \"{1}\".",
                        property.Name, property.GetValue(config)));
                }
            }
            // Adding/updating config to dictionary Configs
            m_Logger.LogInformation(pluginId);
            if (Configs.ContainsKey(pluginId)) Configs[pluginId] = config;
            else Configs.Add(pluginId, config);
            m_Logger.LogDebug(string.Format("\"{0}\" config loaded successfully!", pluginDisplayName));
            return (T)(object)config;
        }
        #endregion LoadConfig Internal

        #region ReloadConfig
        public bool ReloadConfig(string pluginName)
        {
            OpenModUnturnedPlugin plugin = GetPluginByName(pluginName);
            if (plugin == null) return false;
            MainConfig? config = Configs.FirstOrDefault(obj => obj.Key == plugin.OpenModComponentId).Value;
            if (config == null) return false;
            LoadConfig(config, plugin);
            return true;
        }

        public async Task<bool> ReloadConfigAsync(string pluginName)
        {
            OpenModUnturnedPlugin plugin = GetPluginByName(pluginName);
            if (plugin == null) return false;
            MainConfig? config = Configs.FirstOrDefault(obj => obj.Key == plugin.OpenModComponentId).Value;
            if (config == null) return false;
            LoadConfigAsync(config, plugin);
            return true;
        }
        #endregion ReloadConfig

        #region ReloadAllConfigs
        public List<string> ReloadAllConfigs()
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
                LoadConfig(data.Value, plugin);
            }
            m_Logger.LogDebug("Succesfully reloaded configs!");
            return plugins;
        }

        public async Task<List<string>> ReloadAllConfigsAsync()
        {
            m_Logger.LogDebug("Reloading all plugins' configs!");
            List<string> plugins = new();
            Dictionary<string, MainConfig> configsClone = new(Configs);
            List<Task<MainConfig>> tasks = new();
            foreach (KeyValuePair<string, MainConfig> data in configsClone)
            {
                OpenModUnturnedPlugin plugin = GetPluginByName(data.Key);
                plugins.Add(plugin.DisplayName);
                m_Logger.LogInformation(string.Format("Reloading \"{0}\" config!", plugin.DisplayName));
                tasks.Add(LoadConfigAsync(data.Value, plugin));
            }
            await Task.WhenAll(tasks);
            m_Logger.LogDebug("Succesfully reloaded configs!");
            return plugins;
        }
        #endregion ReloadAllConfigs

        public T GetConfig<T>()
        {
            Assembly plugin = Assembly.GetCallingAssembly();
            if (plugin == null)
                throw new Exception("Could not plugin assembly!");
            var metadata = plugin.GetCustomAttribute<PluginMetadataAttribute>();
            if (metadata == null)
                throw new Exception("Could not find plugin metadata!");
            return (T)(object)Configs[metadata.Id];
        }
    }
}
