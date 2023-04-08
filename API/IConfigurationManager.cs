using OpenMod.API.Ioc;
using OpenMod.Unturned.Plugins;
using System.Collections.Generic;
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
        /// Loads plugin config file based on IConfiguration.
        /// </summary>
        /// <typeparam name="T">Config file type extending MainConfig class</typeparam>
        /// <param name="plugin"></param>
        /// <returns>Returns plugin's config.</returns>
        T LoadConfig<T>(OpenModUnturnedPlugin plugin);

        /// <summary>
        /// Loads plugin config file based on IConfiguration.
        /// </summary>
        /// <typeparam name="T">Config file type extending MainConfig class</typeparam>
        /// <param name="config">Config object</param>
        /// <param name="plugin">Plugin</param>
        /// <returns>Returns plugin's config.</returns>
        T LoadConfig<T>(T config, OpenModUnturnedPlugin plugin);

        /// <summary>
        /// Loads plugin config file based on IConfiguration.
        /// </summary>
        /// <typeparam name="T">Config file type extending MainConfig class</typeparam>
        /// <param name="plugin">Plugin</param>
        /// <returns>Returns plugin's config.</returns>
        Task<T> LoadConfigAsync<T>(OpenModUnturnedPlugin plugin);

        /// <summary>
        /// Loads plugin config file based on IConfiguration.
        /// </summary>
        /// <typeparam name="T">Config file type extending MainConfig class</typeparam>
        /// <param name="config"></param>
        /// <param name="plugin">Plugin</param>
        /// <returns>Returns plugin's config.</returns>
        Task<T> LoadConfigAsync<T>(T config, OpenModUnturnedPlugin plugin);

        /// <summary>
        /// Reloads plugin's config using load method.
        /// </summary>
        /// <param name="pluginName">Name of plugin</param>
        /// <returns>Returns true if plugin's config reload was successfull</returns>
        bool ReloadConfig(string pluginName);

        /// <summary>
        /// Reloads plugin's config using load method.
        /// </summary>
        /// <param name="pluginName">Name of plugin</param>
        /// <returns>Returns true if plugin's config reload was successfull.</</returns>
        Task<bool> ReloadConfigAsync(string pluginName);

        /// <summary>
        /// Reloads plugin's config using load method.
        /// </summary>
        /// <returns>Returns list of plugins' names which configs have been reloaded.</returns>
        List<string> ReloadAllConfigs();

        /// <summary>
        /// Reloads plugin's config using load method.
        /// </summary>
        /// <returns>Returns list of plugins' names which configs have been reloaded.</returns>
        Task<List<string>> ReloadAllConfigsAsync();

        /// <summary>
        /// Gets plugin's config.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="plugin"></param>
        /// <returns>Returns plugin's config.</returns>
        T GetConfig<T>(OpenModUnturnedPlugin plugin);

        /// <summary>
        /// Gets properties of plugin's config by plugin object.
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns>Returns config's names and values of properties.</returns>
        List<KeyValuePair<string, string>>? GetConfigProperties(OpenModUnturnedPlugin plugin);

        /// <summary>
        /// Gets properties of plugin's config by name.
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns>Returns config's names and values of properties.</returns>
        List<KeyValuePair<string, string>>? GetConfigProperties(string pluginName);

        /// <summary>
        /// Gets properties of every plugin's config.
        /// </summary>
        /// <returns>Returns dictionary of every plugin's config and it's names and values of properties.</returns>
        Dictionary<OpenModUnturnedPlugin, List<KeyValuePair<string, string>>> GetConfigProperties();
    }
}
