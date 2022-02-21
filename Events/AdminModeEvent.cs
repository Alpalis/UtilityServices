using OpenMod.API.Commands;
using OpenMod.API.Eventing;
using OpenMod.Core.Eventing;
using Steamworks;

namespace Alpalis.UtilityServices.Events
{
    public class AdminModeEvent : Event
    {
        public AdminModeEvent(
            ICommandActor actor)
        {
            Actor = actor;
            SteamID = null;
            IsInAdminMode = true;
        }
        public AdminModeEvent(
            CSteamID steamID)
        {
            SteamID = steamID;
            Actor = null;
            IsInAdminMode = true;
        }

        public ICommandActor? Actor { get; }

        public CSteamID? SteamID { get; }

        public bool IsInAdminMode { get; set; }
    }
}
