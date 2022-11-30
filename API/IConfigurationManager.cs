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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="plugin"></param>
        /// <returns></returns>
        T LoadConfig<T>(OpenModUnturnedPlugin plugin);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="plugin"></param>
        /// <returns></returns>
        T LoadConfig<T>(T config, OpenModUnturnedPlugin plugin);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="plugin"></param>
        /// <returns></returns>
        Task<T> LoadConfigAsync<T>(OpenModUnturnedPlugin plugin);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="plugin"></param>
        /// <returns></returns>
        Task<T> LoadConfigAsync<T>(T config, OpenModUnturnedPlugin plugin);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        bool ReloadConfig(string pluginName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        Task<bool> ReloadConfigAsync(string pluginName);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<string> ReloadAllConfigs();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<string>> ReloadAllConfigsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="plugin"></param>
        /// <returns></returns>
        T GetConfig<T>(OpenModUnturnedPlugin plugin);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns></returns>
        List<KeyValuePair<string, string>>? GetConfigProperties(OpenModUnturnedPlugin plugin);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        List<KeyValuePair<string, string>>? GetConfigProperties(string pluginName);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Dictionary<OpenModUnturnedPlugin, List<KeyValuePair<string, string>>> GetConfigProperties();
    }
}
