using OpenMod.API.Commands;
using OpenMod.Core.Eventing;
using SDG.Unturned;
using Steamworks;

namespace Alpalis.UtilityServices.API.Events
{
    public class SwitchAdminModeEvent : Event
    {
        public SwitchAdminModeEvent(
            SteamPlayer steamPlayer, bool isInAdminMode)
        {
            SteamPlayer = steamPlayer;
            IsInAdminMode = isInAdminMode;
        }
        public SteamPlayer SteamPlayer { get; }

        public bool IsInAdminMode { get; set; }
    }
}
