using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Eventing;
using OpenMod.Core.Eventing;
using OpenMod.Core.Events;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.Events
{
    public class ConsoleWelcomeEvent(
        ILogger<ConsoleWelcomeEvent> logger) : IEventListener<OpenModInitializedEvent>
    {
        private readonly ILogger<ConsoleWelcomeEvent> m_Logger = logger;

        [EventListener(Priority = EventListenerPriority.Normal)]
        public Task HandleEventAsync(object? sender, OpenModInitializedEvent @event)
        {
            m_Logger.LogInformation(new string('=', 45));
            m_Logger.LogInformation("Thanks for using Alpalis Plugins <3!");
            m_Logger.LogInformation("We hope that you will be satisfied with them!");
            m_Logger.LogInformation(new string('=', 45));
            return Task.CompletedTask;
        }
    }
}