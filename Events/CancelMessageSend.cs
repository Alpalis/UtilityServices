using Alpalis.UtilityServices.API.Enums;
using Alpalis.UtilityServices.API.Events;
using OpenMod.API.Eventing;
using OpenMod.API.Plugins;
using OpenMod.Core.Eventing;
using OpenMod.Unturned.Players.Chat.Events;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.Events
{
    public class CancelMessageSend(
        IEventBus eventBus,
        IPluginAccessor<Main> plugin) : IEventListener<UnturnedPlayerChattingEvent>
    {
        private readonly IEventBus m_EventBus = eventBus;
        private readonly Main m_Plugin = plugin.Instance!;

        [EventListener(Priority = EventListenerPriority.Normal)]
        public Task HandleEventAsync(object? sender, UnturnedPlayerChattingEvent @event)
        {
            CanSendMessageEvent checkEvent = new(@event.Player);
            _ = m_EventBus.EmitAsync(m_Plugin, this, checkEvent);
            @event.IsCancelled = checkEvent.IsCancelled;
            if (checkEvent.IsCancelled)
                _ = @event.Player.PrintMessageAsync(checkEvent.Message!);
            return Task.CompletedTask;
        }
    }
}
