using OpenMod.Core.Eventing;

namespace Alpalis.UtilityServices.API.Events.Level
{
    public class PostLevelLoadedEvent(int level) : Event
    {
        public int Level { get; } = level;
    }
}
