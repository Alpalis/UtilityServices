using OpenMod.Core.Eventing;
using SDG.Unturned;

namespace Alpalis.UtilityServices.API.Events
{
    public class SwitchAdminModeEvent(
        SteamPlayer steamPlayer, bool isInAdminMode) : Event
    {
        public SteamPlayer SteamPlayer { get; } = steamPlayer;

        public bool IsInAdminMode { get; set; } = isInAdminMode;
    }
}
