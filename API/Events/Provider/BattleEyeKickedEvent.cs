using OpenMod.Core.Eventing;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.API.Events.Provider
{
    public class BattleEyeKickedEvent : Event
    {
        public SteamPlayer SteamPlayer { get; set; }

        public string Reason { get; set; } = null!;

        public BattleEyeKickedEvent(SteamPlayer steamPlayer, string reason)
        {
            SteamPlayer = steamPlayer;
            Reason = reason;
        }
    }
}
