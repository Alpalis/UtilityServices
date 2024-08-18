using Alpalis.UtilityServices.API.Enums;
using OpenMod.Core.Eventing;
using OpenMod.Unturned.Players;

namespace Alpalis.UtilityServices.API.Events
{
    public class CanSendMessageEvent(UnturnedPlayer player) : Event
    {
        public UnturnedPlayer Player { get; set; } = player;

        public string? Message { get; set; } = null;

        public ECancelMessageReason? Reason { get; set; } = null;

        public bool IsCancelled { get; set; } = false;
    }
}
