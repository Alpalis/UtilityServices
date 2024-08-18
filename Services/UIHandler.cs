using Alpalis.UtilityServices.API;
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Ioc;
using OpenMod.API.Prioritization;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using System.Threading.Tasks;

// to rework and implement openmod IUnturnedUIEffectsKeysProvider

namespace Alpalis.UtilityServices.Services
{
    [ServiceImplementation(Lifetime = ServiceLifetime.Singleton, Priority = Priority.Normal)]
    public class UIHandler : IUIHandler
    {
        public UIHandler()
        {
            TextValues = [];
        }

        private Dictionary<string, Dictionary<string, string>> TextValues { get; set; }

        public Task ClearText(CSteamID steamID)
        {
            TextValues.Remove(steamID.ToString());
            return Task.CompletedTask;
        }

        public Task ClearText(CSteamID steamID, string inputName)
        {
            if (TextValues.ContainsKey(steamID.ToString()))
            {
                if (TextValues[steamID.ToString()].ContainsKey(inputName))
                {
                    if (TextValues[steamID.ToString()].Keys.Count <= 1)
                    {
                        TextValues.Remove(steamID.ToString());
                        return Task.CompletedTask;
                    }
                    TextValues[steamID.ToString()].Remove(inputName);
                }
            }
            return Task.CompletedTask;
        }

        public Task SetText(CSteamID steamID, string inputName, string text)
        {
            if (TextValues.ContainsKey(steamID.ToString()))
            {
                if (TextValues[steamID.ToString()].ContainsKey(inputName))
                {
                    TextValues[steamID.ToString()][inputName] = text;
                    return Task.CompletedTask;
                }
                TextValues[steamID.ToString()].Add(inputName, text);
                return Task.CompletedTask;
            }
            TextValues.Add(steamID.ToString(), new Dictionary<string, string> { { inputName, text } });
            return Task.CompletedTask;
        }

        public bool IsTextSet(CSteamID steamID, string inputName)
        {
            if (!TextValues.TryGetValue(steamID.ToString(), out Dictionary<string, string> playerValue))
                return false;
            if (!playerValue.TryGetValue(inputName, out string value))
                return false;
            if (value == null || value == "")
                return false;
            return true;
        }

        public bool[] IsTextsSet(CSteamID steamID, string[] inputName)
        {
            bool[] value = new bool[inputName.Length];
            if (TextValues.ContainsKey(steamID.ToString()))
            {
                for (ushort a = 0; a < inputName.Length; a++)
                {
                    if (TextValues[steamID.ToString()].ContainsKey(inputName[a]))
                    {
                        string stringValue = GetText(steamID, inputName[a]);
                        if (stringValue != null && stringValue != "")
                        {
                            value[a] = true;
                            continue;
                        }
                        value[a] = false;
                        continue;
                    }
                    value[a] = false;
                    continue;
                }
                return value;
            }
            return value;
        }

        public bool IsPlayerSet(CSteamID steamID)
        {
            return TextValues.ContainsKey(steamID.ToString());
        }

        public string GetText(CSteamID steamID, string inputName)
        {
            return TextValues[steamID.ToString()][inputName];
        }
    }
}
