using Alpalis.UtilityServices.API;
using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.API.Plugins;
using OpenMod.Core.Eventing;
using OpenMod.UnityEngine.Extensions;
using OpenMod.Unturned.Players.Chat.Events;
using OpenMod.Unturned.Players.Connections.Events;
using SDG.Unturned;
using System.Drawing;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.Events
{
    public class WIPWelcomeMessageEvent : IEventListener<UnturnedPlayerConnectedEvent>
    {
        #region Member Variables
        private readonly IConfigurationManager m_ConfigurationManager;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly Main m_Plugin;
        #endregion Member Variables

        #region Class Constructor
        public WIPWelcomeMessageEvent(
            IConfigurationManager configurationManager,
            IStringLocalizer stringLocalizer,
            IPluginAccessor<Main> plugin)
        {
            m_ConfigurationManager = configurationManager;
            m_StringLocalizer = stringLocalizer;
            m_Plugin = plugin.Instance!;
        }
        #endregion Class Constructor

        [EventListener(Priority = EventListenerPriority.Normal)]
        public async Task HandleEventAsync(object? sender, UnturnedPlayerConnectedEvent @event)
        {
            SteamPlayer sPlayer = @event.Player.SteamPlayer;
            ChatManager.serverSendMessage(m_StringLocalizer["welcome_message:prefix"],
                Color.LimeGreen.ToUnityColor(),
                null, sPlayer, EChatMode.SAY, null, true);
            ChatManager.serverSendMessage(m_StringLocalizer["welcome_message:message"],
                Color.White.ToUnityColor(),
                null, sPlayer, EChatMode.SAY, null, true);
            ChatManager.serverSendMessage(m_StringLocalizer["welcome_message:suffix"],
                Color.LimeGreen.ToUnityColor(),
                null, sPlayer, EChatMode.SAY, null, true);
        }
    }
}
