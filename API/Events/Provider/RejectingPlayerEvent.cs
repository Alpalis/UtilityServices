using OpenMod.Core.Eventing;
using SDG.Unturned;
using Steamworks;

namespace Alpalis.UtilityServices.API.Events.Provider
{
    public class RejectingPlayerEvent(CSteamID steamID, ESteamRejection rejection, string explanation) : Event
    {
        public CSteamID SteamId { get; } = steamID;

        public ESteamRejection Rejection { get; } = rejection;

        public string Explanation { get; } = explanation;
    }
}
