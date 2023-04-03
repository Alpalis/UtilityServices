using OpenMod.Core.Eventing;
using Steamworks;

namespace Alpalis.UtilityServices.API.Events
{
    public class GetIdentityEvent : Event
    {
        public GetIdentityEvent(
            CSteamID steamID)
        {
            SteamID = steamID;
            Identity = null;
        }

        public CSteamID SteamID { get; }

        public ushort? Identity { get; }
    }
}
