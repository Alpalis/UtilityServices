using Alpalis.UtilityServices.API.Enums;
using Alpalis.UtilityServices.API.Events;
using OpenMod.API.Eventing;
using OpenMod.API.Plugins;
using OpenMod.Core.Eventing;
using OpenMod.Unturned.Players.Chat.Events;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.Events
{
    public class CancelMessageSend : IEventListener<UnturnedPlayerChattingEvent>
    {
        #region Member Variables
        private readonly IEventBus m_EventBus;
        private readonly Main m_Plugin;
        #endregion Member Variables

        #region Class Constructor
        public CancelMessageSend(
            IEventBus eventBus,
            IPluginAccessor<Main> plugin)
        {
            m_EventBus = eventBus;
            m_Plugin = plugin.Instance!;
        }
        #endregion Class Constructor

        [EventListener(Priority = EventListenerPriority.Normal)]
        public async Task HandleEventAsync(object? sender, UnturnedPlayerChattingEvent @event)
        {
            CanSendMessageEvent checkEvent = new(@event.Player);
            m_EventBus.EmitAsync(m_Plugin, this, checkEvent);
            @event.IsCancelled = checkEvent.IsCancelled;
            if (checkEvent.IsCancelled) @event.Player.PrintMessageAsync(checkEvent.Message!);
        }
    }
}
