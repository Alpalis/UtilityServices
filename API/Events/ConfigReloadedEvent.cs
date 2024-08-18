using Alpalis.UtilityServices.Models;
using OpenMod.Core.Eventing;
using OpenMod.Unturned.Plugins;

namespace Alpalis.UtilityServices.API.Events
{
    public class ConfigReloadedEvent(OpenModUnturnedPlugin plugin, MainConfig config) : Event
    {
        public OpenModUnturnedPlugin Plugin { get; } = plugin;

        public MainConfig Config { get; internal set; } = config;
    }
}
