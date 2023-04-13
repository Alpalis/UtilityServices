using Alpalis.UtilityServices.API;
using Alpalis.UtilityServices.Models;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenMod.API.Ioc;
using OpenMod.API.Prioritization;
using OpenMod.Core.Helpers;
using OpenMod.Unturned.Effects;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.Services
{
    /*[ServiceImplementation(Lifetime = ServiceLifetime.Singleton, Priority = Priority.Normal)]
    public class UIManager : IUIManager
    {
        private readonly ILogger<UIManager> m_Logger;
        private readonly IUnturnedUIEffectsKeysProvider m_UnturnedUIEffectsKeysProvider;

        public UIManager(
            ILogger<UIManager> logger,
            IUnturnedUIEffectsKeysProvider unturnedUIEffectsKeysProvider)
        {
            m_Logger = logger;
            m_UnturnedUIEffectsKeysProvider = unturnedUIEffectsKeysProvider;
        }

        public async UniTask RunMainUI()
        {
        }


        public async UniTask RunMainUI(SteamPlayer sPlayer, ushort ID, short key)
        {
            CSteamID steamID = sPlayer.playerID.steamID;
            await UniTask.SwitchToMainThread();
            if (EnabledUIs.ContainsKey(steamID.m_SteamID))
            {
                if (EnabledUIs[steamID.m_SteamID].Any(ui => ui.ID == ID))
                    return;
                foreach (EnabledUI enabledUI in EnabledUIs[steamID.m_SteamID])
                {
                    EffectManager.askEffectClearByID(enabledUI.ID, sPlayer.transportConnection);
                    if (enabledUI.IsMain)
                    {
                        m_Logger.LogDebug(string.Format("The Main UI has been removed for the player {0} ({1}).",
                            sPlayer.playerID.characterName, steamID));
                        RemoveFromList(sPlayer.playerID.steamID, enabledUI.ID);
                        continue;
                    }
                    m_Logger.LogDebug(string.Format("The Side UI has been hidden from the player {0} ({1}).",
                        sPlayer.playerID.characterName, steamID));
                    enabledUI.Visible = false;
                }
            }
            sPlayer.player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);
            m_Logger.LogDebug(string.Format("The Main UI was displayed to the player {0} ({1}).",
                sPlayer.playerID.characterName, steamID));
            AddToList(steamID, ID, key, true, true);
            EffectManager.sendUIEffect(ID, key, sPlayer.transportConnection, true);
        }

        public async UniTask RunSideUI(SteamPlayer sPlayer, ushort ID, short key)
        {
            CSteamID steamID = sPlayer.playerID.steamID;
            if (EnabledUIs.ContainsKey(steamID.m_SteamID))
            {
                foreach (EnabledUI enabledUI in EnabledUIs[steamID.m_SteamID])
                {
                    if (!enabledUI.IsMain) continue;
                    m_Logger.LogDebug(string.Format("The Side UI was added to the player {0} ({1}) but not displayed.",
                        sPlayer.playerID.characterName, steamID));
                    AddToList(steamID, ID, key, false, false);
                    return;
                }
            }
            m_Logger.LogDebug(string.Format("The Side UI was displayed to the player {0} ({1}).",
                sPlayer.playerID.characterName, steamID));
            AddToList(steamID, ID, key, true, false);
            await UniTask.SwitchToMainThread();
            EffectManager.sendUIEffect(ID, key, sPlayer.transportConnection, true);
        }

        public async UniTask StopMainUI(SteamPlayer sPlayer, ushort ID)
        {
            CSteamID steamID = sPlayer.playerID.steamID;
            RemoveFromList(steamID, ID);
            await UniTask.SwitchToMainThread();
            sPlayer.player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
            foreach (EnabledUI enabledUI in EnabledUIs[steamID.m_SteamID])
            {
                if (enabledUI.IsMain) continue;
                m_Logger.LogDebug(string.Format("The Side UI has been restored to the player {0} ({1}).",
                    sPlayer.playerID.characterName, steamID));
                enabledUI.Visible = true;
                EffectManager.sendUIEffect(enabledUI.ID, enabledUI.Key, sPlayer.transportConnection, true);
            }
            m_Logger.LogDebug(string.Format("The Main UI has been removed for the player {0} ({1}).",
                sPlayer.playerID.characterName, steamID));
            EffectManager.askEffectClearByID(ID, sPlayer.transportConnection);
        }

        public async UniTask StopSideUI(SteamPlayer sPlayer, ushort ID)
        {
            CSteamID steamID = sPlayer.playerID.steamID;
            m_Logger.LogDebug(string.Format("The Side UI has been removed for the player {0} ({1}).",
                sPlayer.playerID.characterName, steamID));
            RemoveFromList(steamID, ID);
            await UniTask.SwitchToMainThread();
            EffectManager.askEffectClearByID(ID, sPlayer.transportConnection);
        }

        public async UniTask StopSideUI(SteamPlayer sPlayer, ushort ID, short key, string name, int time)
        {
            CSteamID steamID = sPlayer.playerID.steamID;
            RemoveFromList(steamID, ID);
            m_Logger.LogDebug(string.Format("The Side UI has been removed for the player {0} ({1}).",
                    sPlayer.playerID.characterName, steamID));
            AsyncHelper.Schedule("UIAnimation", async () =>
            {
                await UniTask.SwitchToMainThread();
                EffectManager.sendUIEffectVisibility(key, sPlayer.transportConnection, true, $"{name}Entry", false);
                EffectManager.sendUIEffectVisibility(key, sPlayer.transportConnection, true, $"{name}Exit", true);
                await UniTask.Delay(time);
                if (IsUIEnabled(steamID, ID)) return;
                EffectManager.askEffectClearByID(ID, sPlayer.transportConnection);
            });
        }

        public async UniTask DisplayWarning(SteamPlayer sPlayer, ushort ID, string visibilityChildName, string textChildName, string text, int time)
        {
            m_Logger.LogDebug(string.Format("The Warning was displayed to the player {0} ({1}).",
                sPlayer.playerID.characterName, sPlayer.playerID.steamID));
            CSteamID steamID = sPlayer.playerID.steamID;
            if (IsWarningEnabled(steamID, visibilityChildName) || !IsUIEnabled(steamID, ID)) return;
            AddWarning(steamID, visibilityChildName);
            await UniTask.SwitchToMainThread();
            short key = EnabledUIs[steamID.m_SteamID].Find(UI => UI.ID == ID).Key;
            EffectManager.sendUIEffectVisibility(key, sPlayer.transportConnection, true, visibilityChildName, true);
            EffectManager.sendUIEffectText(key, sPlayer.transportConnection, true, textChildName, text);
            await UniTask.Delay(time);
            m_Logger.LogDebug(string.Format("The Warning was removed for the player {0} ({1}).",
                sPlayer.playerID.characterName, steamID));
            RemoveWarning(steamID, visibilityChildName);
            if (!IsUIEnabled(steamID, ID)) return;
            EffectManager.sendUIEffectVisibility(key, sPlayer.transportConnection, true, visibilityChildName, false);
        }

        public async UniTask DisplayError(SteamPlayer sPlayer, byte errorCode)
        {
            CSteamID steamID = sPlayer.playerID.steamID;
            if (IsErrorEnabled(steamID)) return;
            EnabledErrors.Add(steamID.m_SteamID);
            sPlayer.player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, 
                EnabledUIs.ContainsKey(steamID.m_SteamID) && !EnabledUIs[steamID.m_SteamID].Any(enabledUI => enabledUI.IsMain));
            string date = DateTime.Now.ToString("HH:mm dd/MM/yyyy");
            string errorCodeString = errorCode.ToString("000");
            m_Logger.LogDebug(string.Format("The Error was displayed to the player {0} ({1}).",
                sPlayer.playerID.characterName, sPlayer.playerID.steamID));
            EffectManager.sendUIEffect(29044, 44, sPlayer.transportConnection, true);
            EffectManager.sendUIEffectText(44, sPlayer.transportConnection, true, "Date", date);
            EffectManager.sendUIEffectText(44, sPlayer.transportConnection, true, "BackDate1", date);
            EffectManager.sendUIEffectText(44, sPlayer.transportConnection, true, "BackDate2", date);
            EffectManager.sendUIEffectText(44, sPlayer.transportConnection, true, "BackDate3", date);
            EffectManager.sendUIEffectText(44, sPlayer.transportConnection, true, "BackDate4", date);
            EffectManager.sendUIEffectText(44, sPlayer.transportConnection, true, "SteamID", sPlayer.playerID.playerName + " (" + steamID + ")");
            EffectManager.sendUIEffectText(44, sPlayer.transportConnection, true, "ErrorCode", "Error " + errorCodeString);
        }

        public async UniTask CloseError(SteamPlayer sPlayer, bool useEffectManager)
        {
            CSteamID steamID = sPlayer.playerID.steamID;
            EnabledErrors.Remove(steamID.m_SteamID);
            if (!useEffectManager) return;
            if (EnabledUIs.ContainsKey(steamID.m_SteamID))
            {
                if (!EnabledUIs[steamID.m_SteamID].Any(enabledUI => enabledUI.IsMain))
                {
                    sPlayer.player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
                }
            }
            else
            {
                sPlayer.player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
            }
            EffectManager.askEffectClearByID(29044, sPlayer.transportConnection);
        }

        public bool IsErrorEnabled(CSteamID steamID)
        {
            return EnabledErrors.Contains(steamID.m_SteamID);
        }

        public bool IsWarningEnabled(CSteamID steamID, string visibilityChildName)
        {
            if (EnabledWarnings.ContainsKey(steamID.m_SteamID))
            {
                if (EnabledWarnings[steamID.m_SteamID].Contains(visibilityChildName))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private async Task AddWarning(CSteamID steamID, string visibilityChildName)
        {
            if (EnabledWarnings.ContainsKey(steamID.m_SteamID))
            {
                if (EnabledWarnings[steamID.m_SteamID].Contains(visibilityChildName)) return;
                EnabledWarnings[steamID.m_SteamID].Add(visibilityChildName);
            }
            EnabledWarnings.Add(steamID.m_SteamID, new List<string>());
            EnabledWarnings[steamID.m_SteamID].Add(visibilityChildName);
        }

        private async Task RemoveWarning(CSteamID steamID, string visibilityChildName)
        {
            if (!EnabledWarnings.ContainsKey(steamID.m_SteamID))
                return;
            if (!EnabledWarnings[steamID.m_SteamID].Contains(visibilityChildName))
                return;
            if (EnabledWarnings[steamID.m_SteamID].Count == 1)
            {
                EnabledWarnings.Remove(steamID.m_SteamID);
                return;
            }
            EnabledWarnings[steamID.m_SteamID].Remove(visibilityChildName);
        }

        public async UniTask StopAllUI(SteamPlayer sPlayer, bool useEffectManager)
        {
            if (useEffectManager)
            {
                await UniTask.SwitchToMainThread();
                foreach (EnabledUI enabledUI in EnabledUIs[sPlayer.playerID.steamID.m_SteamID])
                {
                    EffectManager.askEffectClearByID(enabledUI.ID, sPlayer.transportConnection);
                }
            }
            RemoveFromList(sPlayer.playerID.steamID);
        }

        private async Task AddToList(CSteamID steamID, ushort ID, short key, bool visible, bool isMain)
        {
            if (EnabledUIs.ContainsKey(steamID.m_SteamID))
            {
                EnabledUI UI = EnabledUIs[steamID.m_SteamID].Find(UI => UI.ID == ID);
                if (UI == null)
                {
                    EnabledUIs[steamID.m_SteamID].Add(new EnabledUI
                    {
                        ID = ID,
                        Key = key,
                        Visible = visible,
                        IsMain = isMain
                    });
                    return;
                }
                return;
            }
            EnabledUIs.Add(steamID.m_SteamID, new List<EnabledUI>
            {
                new EnabledUI
                {
                    ID = ID,
                    Key = key,
                    Visible = visible,
                    IsMain = isMain
                }
            });
        }

        public bool IsUIEnabled(CSteamID steamID, ushort ID)
        {
            if (EnabledUIs.ContainsKey(steamID.m_SteamID))
            {
                EnabledUI UI = EnabledUIs[steamID.m_SteamID].Find(UI => UI.ID == ID);
                if (UI != null)
                {
                    if (UI.Visible)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        private async Task RemoveFromList(CSteamID steamID, ushort ID)
        {
            if (!EnabledUIs.ContainsKey(steamID.m_SteamID))
                return;
            EnabledUI UI = EnabledUIs[steamID.m_SteamID].Find(UI => UI.ID == ID);
            if (UI == null)
                return;
            if (EnabledUIs[steamID.m_SteamID].Count > 1)
            {
                EnabledUIs[steamID.m_SteamID].Remove(UI);
                return;
            }
            EnabledUIs.Remove(steamID.m_SteamID);
        }
        private async Task RemoveFromList(CSteamID steamID)
        {
            if (!EnabledUIs.ContainsKey(steamID.m_SteamID))
                return;
            EnabledUIs.Remove(steamID.m_SteamID);
        }
    }*/
}
