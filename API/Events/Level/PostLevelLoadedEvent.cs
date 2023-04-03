using OpenMod.Core.Eventing;

namespace Alpalis.UtilityServices.API.Events.Level
{
    public class PostLevelLoadedEvent : Event
    {
        public int Level { get; set; }

        public PostLevelLoadedEvent(int level)
        {
            Level = level;
        }
    }
}
