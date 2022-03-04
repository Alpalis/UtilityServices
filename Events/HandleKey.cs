using Alpalis.UtilityServices.API;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenMod.API.Eventing;
using SDG.Unturned;
using Steamworks;
using System;

namespace Alpalis.UtilityServices.Events
{
    public class HandleKey : IDisposable
    {
        #region Member Variables
        private readonly IKeyHandler m_KeyHandler;
        private readonly ILogger<HandleKey> m_Logger;
        private readonly IEventBus m_EventBus;
        #endregion Member Variables

        #region Class Constructor
        public HandleKey(
            IKeyHandler keyHandler,
            ILogger<HandleKey> logger,
            IEventBus eventBus)
        {
            m_KeyHandler = keyHandler;
            m_Logger = logger;
            m_EventBus = eventBus;
            PlayerInput.onPluginKeyTick += OnKeyPressed;
        }
        #endregion Class Constructor

        private async void OnKeyPressed(Player player, uint simulation, byte key, bool state)
        {
            await UniTask.SwitchToThreadPool();
            SteamPlayer sPlayer = player.channel.owner;
            CSteamID steamID = sPlayer.playerID.steamID;
            bool keyState = m_KeyHandler.GetKeyState(steamID, key);
            if (state)
            {
                if (keyState) return;
                if (m_KeyHandler.GetKeyCooldown(steamID, key)) return;
                await m_KeyHandler.SetKeyDown(steamID, key);
                switch (key)
                {
                    case 0:
                        m_Logger.LogDebug(string.Format("The player {0} ({1}) pressed a key(,).", sPlayer.playerID.characterName, steamID));
                        break;
                    case 1:
                        m_Logger.LogDebug(string.Format("The player {0} ({1}) pressed a key(.).", sPlayer.playerID.characterName, steamID));
                        break;
                    case 2:
                        m_Logger.LogDebug(string.Format("The player {0} ({1}) pressed a key(/).", sPlayer.playerID.characterName, steamID));
                        break;
                    case 3:
                        m_Logger.LogDebug(string.Format("The player {0} ({1}) pressed a key(;).", sPlayer.playerID.characterName, steamID));
                        break;
                    case 4:
                        m_Logger.LogDebug(string.Format("The player {0} ({1}) pressed a key(').", sPlayer.playerID.characterName, steamID));
                        break;
                }
                return;

            }
            if (!keyState) return;
            await m_KeyHandler.SetKeyUp(steamID, key);
            switch (key)
            {
                case 0:
                    m_Logger.LogDebug(string.Format("The player {0} ({1}) released a key(,).", sPlayer.playerID.characterName, steamID));
                    break;
                case 1:
                    m_Logger.LogDebug(string.Format("The player {0} ({1}) released a key(.).", sPlayer.playerID.characterName, steamID));
                    break;
                case 2:
                    m_Logger.LogDebug(string.Format("The player {0} ({1}) released a key(/).", sPlayer.playerID.characterName, steamID));
                    break;
                case 3:
                    m_Logger.LogDebug(string.Format("The player {0} ({1}) released a key(;).", sPlayer.playerID.characterName, steamID));
                    break;
                case 4:
                    m_Logger.LogDebug(string.Format("The player {0} ({1}) released a key(').", sPlayer.playerID.characterName, steamID));
                    break;
            }
            return;
        }

        public void Dispose()
        {
            PlayerInput.onPluginKeyTick -= OnKeyPressed;
        }
    }
}
