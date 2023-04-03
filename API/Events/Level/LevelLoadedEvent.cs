using OpenMod.Core.Eventing;

namespace Alpalis.UtilityServices.API.Events.Level
{
    public class LevelLoadedEvent : Event
    {
        public int Level { get; set; }

        public LevelLoadedEvent(int level)
        {
            Level = level;
        }
    }
}
