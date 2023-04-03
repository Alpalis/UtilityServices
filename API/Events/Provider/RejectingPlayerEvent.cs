using OpenMod.Core.Eventing;
using SDG.Unturned;
using Steamworks;

namespace Alpalis.UtilityServices.API.Events.Provider
{
    public class RejectingPlayerEvent : Event
    {
        public CSteamID SteamId { get; set; }

        public ESteamRejection Rejection { get; set; }

        public string Explanation { get; set; } = null!;

        public RejectingPlayerEvent(CSteamID steamID, ESteamRejection rejection, string explanation)
        {
            SteamId = steamID;
            Rejection = rejection;
            Explanation = explanation;
        }
    }
}
