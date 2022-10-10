using Alpalis.UtilityServices.Models;
using Microsoft.Extensions.Configuration;
using OpenMod.API.Ioc;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.API
{
    /// <summary>
    /// Interface for managing configs' data of plugins.
    /// </summary>
    [Service]
    public interface IConfigurationManager
    {
        /// <summary>
        /// Load plugin's config data to memory.
        /// </summary>
        T LoadConfig<T>();

        T LoadConfig<T>(T config);

        T LoadConfig<T>(OpenModUnturnedPlugin plugin);

        T LoadConfig<T>(T config, OpenModUnturnedPlugin plugin);

        Task<T> LoadConfigAsync<T>(OpenModUnturnedPlugin plugin);

        Task<T> LoadConfigAsync<T>(T config, OpenModUnturnedPlugin plugin);

        /// <summary>
        /// Reloads config of specified plugin.
        /// </summary>
        /// <param name="pluginName">Full displayname of plugin</param>
        /// <returns>Return if reloading of plugin's config is successful</returns>
        bool ReloadConfig(string pluginName);

        Task<bool> ReloadConfigAsync(string pluginName);

        /// <summary>
        /// Reloads all plugin' configs.
        /// </summary>
        /// <returns>List of plugins' names that configs are reloaded</returns>
        List<string> ReloadAllConfigs();

        Task<List<string>> ReloadAllConfigsAsync();

        /// <summary>
        /// Returns config of specified plugin.
        /// </summary>
        /// <typeparam name="T">Data class with config structure</typeparam>
        /// <param name="plugin">Specified plugin's data</param>
        /// <returns>Data class that extends MainConfig</returns>
        T GetConfig<T>();
    }
}
