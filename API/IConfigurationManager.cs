using Alpalis.UtilityServices.Models;
using OpenMod.API.Ioc;
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
        /// <param name="plugin">Specified plugin's data</param>
        /// <param name="config">Config of specified plugin</param>
        Task LoadConfig(OpenModUnturnedPlugin plugin, MainConfig config);
        
        /// <summary>
        /// Reloads config of specified plugin.
        /// </summary>
        /// <param name="pluginName">Full displayname of plugin</param>
        Task ReloadConfig(string pluginName);

        /// <summary>
        /// Reloads all plugin' configs.
        /// </summary>
        Task ReloadAllConfig();

        /// <summary>
        /// Returns config of specified plugin.
        /// </summary>
        /// <typeparam name="T">Data class with config structure</typeparam>
        /// <param name="plugin">Specified plugin's data</param>
        /// <returns>Data class that extends MainConfig</returns>
        T GetConfig<T>(OpenModUnturnedPlugin plugin);
    }
}
