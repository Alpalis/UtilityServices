using Alpalis.UtilityServices.Models;
using OpenMod.API.Commands;
using OpenMod.Core.Eventing;
using OpenMod.Unturned.Plugins;
using Steamworks;

namespace Alpalis.UtilityServices.Events
{
    public class ConfigReloadedEvent : Event
    {
        public ConfigReloadedEvent(OpenModUnturnedPlugin plugin, MainConfig config)
        {
            Plugin = plugin;
            Config = config;
        }

        public OpenModUnturnedPlugin Plugin { get; }

        public MainConfig Config { get; set; }
    }
}
