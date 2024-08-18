using OpenMod.Core.Eventing;
using Steamworks;

namespace Alpalis.UtilityServices.API.Events
{
    public class GetIdentityEvent(
        CSteamID steamID) : Event
    {
        public CSteamID SteamID { get; } = steamID;

        public ushort? Identity { get; } = null;
    }
}
