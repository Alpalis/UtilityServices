using OpenMod.Core.Eventing;

namespace Alpalis.UtilityServices.API.Events.Level
{
    public class PreLevelLoadedEvent : Event
    {
        public int Level { get; set; }

        public PreLevelLoadedEvent(int level)
        {
            Level = level;
        }
    }
}
