using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;
using System.Linq;

namespace Alpalis.UtilityServices
{
    /// <summary>
    /// 
    /// </summary>
    public static class PluginActivatorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluginActivator"></param>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        public static OpenModUnturnedPlugin? GetPluginByName(this IPluginActivator pluginActivator, string pluginName) =>
            (OpenModUnturnedPlugin?)pluginActivator.ActivatedPlugins.FirstOrDefault(name => name.DisplayName.Equals(pluginName, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluginActivator"></param>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        public static OpenModUnturnedPlugin? GetPluginBySimmilarName(this IPluginActivator pluginActivator, string pluginName) =>
            (OpenModUnturnedPlugin?)pluginActivator.ActivatedPlugins.FirstOrDefault(name => name.DisplayName.IndexOf(pluginName, StringComparison.OrdinalIgnoreCase) != -1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluginActivator"></param>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        public static OpenModUnturnedPlugin? GetPluginById(this IPluginActivator pluginActivator, string pluginId) =>
            (OpenModUnturnedPlugin?)pluginActivator.ActivatedPlugins.FirstOrDefault(name => name.OpenModComponentId.Equals(pluginId, StringComparison.Ordinal));
    }
}
