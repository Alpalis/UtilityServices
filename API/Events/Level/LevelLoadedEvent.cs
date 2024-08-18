using OpenMod.Core.Eventing;

namespace Alpalis.UtilityServices.API.Events.Level
{
    public class LevelLoadedEvent(int level) : Event
    {
        public int Level { get; } = level;
    }
}
