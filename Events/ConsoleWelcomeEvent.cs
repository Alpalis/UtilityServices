using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Eventing;
using OpenMod.API.Plugins;
using OpenMod.Core.Eventing;
using OpenMod.Core.Events;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.Events
{
    public class ConsoleWelcomeEvent : IEventListener<OpenModInitializedEvent>
    {
        #region Member Variables
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly ILogger<ConsoleWelcomeEvent> m_Logger;
        private readonly Main m_Plugin;
        #endregion Member Variables

        #region Class Constructor
        public ConsoleWelcomeEvent(
            IStringLocalizer stringLocalizer,
            ILogger<ConsoleWelcomeEvent> logger,
            IPluginAccessor<Main> plugin)
        {
            m_StringLocalizer = stringLocalizer;
            m_Logger = logger;
            m_Plugin = plugin.Instance!;
        }
        #endregion Class Constructor

        [EventListener(Priority = EventListenerPriority.Normal)]
        public async Task HandleEventAsync(object? sender, OpenModInitializedEvent @event)
        {
            m_Logger.LogInformation(new string('=', 45));
            m_Logger.LogInformation("Thanks for using Alpalis Plugins <3!");
            m_Logger.LogInformation("We hope that you will be satisfied with them!");
            m_Logger.LogInformation(new string('=', 45));
        }
    }
}