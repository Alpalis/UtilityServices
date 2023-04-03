using Alpalis.UtilityServices.API.Enums;
using OpenMod.Core.Eventing;
using OpenMod.Unturned.Players;

namespace Alpalis.UtilityServices.API.Events
{
    public class CanSendMessageEvent : Event
    {
        public CanSendMessageEvent(UnturnedPlayer player)
        {
            Player = player;
            Reason = null;
            IsCancelled = false;
            Message = null;
        }

        public UnturnedPlayer Player { get; set; }

        public string? Message { get; set; }

        public ECancelMessageReason? Reason { get; set; }

        public bool IsCancelled { get; set; }
    }
}
