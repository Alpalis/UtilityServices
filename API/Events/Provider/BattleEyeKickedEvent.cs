using OpenMod.Core.Eventing;
using SDG.Unturned;

namespace Alpalis.UtilityServices.API.Events.Provider
{
    public class BattleEyeKickedEvent(SteamPlayer steamPlayer, string reason) : Event
    {
        public SteamPlayer SteamPlayer { get; } = steamPlayer;

        public string Reason { get; } = reason;
    }
}
