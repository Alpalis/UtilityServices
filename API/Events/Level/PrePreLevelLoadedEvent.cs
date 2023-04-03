using OpenMod.Core.Eventing;

namespace Alpalis.UtilityServices.API.Events.Level
{
    public class PrePreLevelLoadedEvent : Event
    {
        public int Level { get; set; }

        public PrePreLevelLoadedEvent(int level)
        {
            Level = level;
        }
    }
}
