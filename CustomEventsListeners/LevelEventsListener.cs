using Alpalis.UtilityServices.API.CustomEventsListeners;
using Alpalis.UtilityServices.API.Events.Level;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.CustomEventsListeners
{
    internal class LevelEventsListener : CustomEventsListener
    {
        public LevelEventsListener(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override void Subscribe()
        {
            Level.onLevelExited += OnLevelExited;
            Level.onLevelsRefreshed += OnLevelsRefreshed;
            Level.onPostLevelLoaded += OnPostLevelLoaded;
            Level.onPreLevelLoaded += OnPreLevelLoaded;
            Level.onPrePreLevelLoaded += OnPrePreLevelLoaded;
            Level.onLevelLoaded += OnLevelLoaded;
        }
        private void OnLevelExited()
        {
            LevelExitedEvent @event = new();
            Emit(@event);
        }

        private void OnLevelsRefreshed()
        {
            LevelsRefreshedEvent @event = new();
            Emit(@event);
        }

        private void OnPostLevelLoaded(int level)
        {
            PostLevelLoadedEvent @event = new(level);
            Emit(@event);
        }

        private void OnPreLevelLoaded(int level)
        {
            PreLevelLoadedEvent @event = new(level);
            Emit(@event);
        }

        private void OnPrePreLevelLoaded(int level)
        {
            PrePreLevelLoadedEvent @event = new(level);
            Emit(@event);
        }

        private void OnLevelLoaded(int level)
        {
            LevelLoadedEvent @event = new(level);
            Emit(@event);
        }

        public override void Unsubscribe()
        {
            Level.onLevelExited -= OnLevelExited;
            Level.onLevelsRefreshed -= OnLevelsRefreshed;
            Level.onPostLevelLoaded -= OnPostLevelLoaded;
            Level.onPreLevelLoaded -= OnPreLevelLoaded;
            Level.onPrePreLevelLoaded -= OnPrePreLevelLoaded;
            Level.onLevelLoaded -= OnLevelLoaded;
        }
    }
}
