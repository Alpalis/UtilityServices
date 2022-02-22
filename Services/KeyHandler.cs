using Alpalis.UtilityServices.API;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Ioc;
using OpenMod.API.Prioritization;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.Services
{
    [ServiceImplementation(Lifetime = ServiceLifetime.Singleton, Priority = Priority.Normal)]
    public class KeyHandler : IKeyHandler
    {
        #region Class Constructor
        public KeyHandler()
        {
            KeyStatusTable = new Dictionary<string, List<int>>();
            Cooldown = new Dictionary<string, Dictionary<int, DateTime>>();
        }
        #endregion Class Constructor

        private Dictionary<string, List<int>> KeyStatusTable { get; set; }

        private Dictionary<string, Dictionary<int, DateTime>> Cooldown { get; set; }

        public async Task SetKeyDown(CSteamID steamID, int key)
        {
            await UniTask.SwitchToThreadPool();
            if (!KeyStatusTable.ContainsKey(steamID.ToString()))
            {
                KeyStatusTable.Add(steamID.ToString(), new List<int> { { key } });
                return;
            }
            if (!KeyStatusTable[steamID.ToString()].Contains(key))
            {
                KeyStatusTable[steamID.ToString()].Add(key);
                return;
            }
        }

        public async Task SetKeyUp(CSteamID steamID, int key)
        {
            await UniTask.SwitchToThreadPool();
            if (!KeyStatusTable.ContainsKey(steamID.ToString()))
                return;
            if (!KeyStatusTable[steamID.ToString()].Contains(key))
                return;
            if (Cooldown.ContainsKey(steamID.ToString()))
            {
                if (!Cooldown[steamID.ToString()].ContainsKey(key))
                {
                    Cooldown[steamID.ToString()].Add(key, DateTime.UtcNow);
                    return;
                }
                else
                {
                    Cooldown[steamID.ToString()][key] = DateTime.UtcNow;
                }
            }
            else
            {
                Cooldown.Add(steamID.ToString(), new Dictionary<int, DateTime> { { key, DateTime.UtcNow } });
            }
            KeyStatusTable[steamID.ToString()].Remove(key);
            return;
        }

        public bool GetKeyState(CSteamID steamID, int key)
        {
            if (KeyStatusTable.ContainsKey(steamID.ToString()))
            {
                if (KeyStatusTable[steamID.ToString()].Contains(key))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool GetKeyCooldown(CSteamID steamID, int key)
        {
            if (Cooldown.ContainsKey(steamID.ToString()))
            {
                if (Cooldown[steamID.ToString()].ContainsKey(key))
                {
                    if (DateTime.UtcNow.Subtract(Cooldown[steamID.ToString()][key]).TotalSeconds < 2)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }
    }
}