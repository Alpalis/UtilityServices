using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices
{
    public static class PluginActivatorExtensions
    {
        public static OpenModUnturnedPlugin? GetPluginByName(this IPluginActivator pluginActivator, string pluginName) =>
            (OpenModUnturnedPlugin?)pluginActivator.ActivatedPlugins.FirstOrDefault(name => name.DisplayName.Equals(pluginName, StringComparison.OrdinalIgnoreCase));

        public static OpenModUnturnedPlugin? GetPluginBySimmilarName(this IPluginActivator pluginActivator, string pluginName) =>
            (OpenModUnturnedPlugin?)pluginActivator.ActivatedPlugins.FirstOrDefault(name => name.DisplayName.IndexOf(pluginName, StringComparison.OrdinalIgnoreCase) != -1);

        public static OpenModUnturnedPlugin? GetPluginById(this IPluginActivator pluginActivator, string pluginId) =>
            (OpenModUnturnedPlugin?)pluginActivator.ActivatedPlugins.FirstOrDefault(name => name.OpenModComponentId.Equals(pluginId, StringComparison.Ordinal));
    }
}
