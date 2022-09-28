using Cysharp.Threading.Tasks;
using OpenMod.API.Ioc;
using SDG.Unturned;
using Steamworks;

namespace Alpalis.UtilityServices.API
{
    /// <summary>
    /// Inteface for managing the player's UI.
    /// </summary>
    [Service]
    public interface IUIManager
    {
        /// <summary>
        /// Starts the main UI for the player.
        /// </summary>
        /// <param name="sPlayer">SteamPlayer</param>
        /// <param name="ID">Asset ID of UI</param>
        /// <param name="key">Key of UI</param>
        UniTask RunMainUI(SteamPlayer sPlayer, ushort ID, short key);

        /// <summary>
        /// Starts the side UI for the player.
        /// </summary>
        /// <param name="sPlayer">SteamPlayer</param>
        /// <param name="ID">Asset ID of UI</param>
        /// <param name="key">Key of UI</param>
        UniTask RunSideUI(SteamPlayer sPlayer, ushort ID, short key);

        /// <summary>
        /// Stops the main UI for the player.
        /// </summary>
        /// <param name="sPlayer">SteamPlayer</param>
        /// <param name="ID">Asset ID of UI</param>
        UniTask StopMainUI(SteamPlayer sPlayer, ushort ID);

        /// <summary>
        /// Stops the side UI for the player.
        /// </summary>
        /// <param name="sPlayer">SteamPlayer</param>
        /// <param name="ID">Asset ID of UI</param>
        UniTask StopSideUI(SteamPlayer sPlayer, ushort ID);

        /// <summary>
        /// Stops the side UI for the player with animation. It's disabling name + Entry object and enabling name + Exit object
        /// </summary>
        /// <param name="sPlayer">SteamPlayer</param>
        /// <param name="ID">Asset ID of UI</param>
        /// <param name="key">Key of UI</param>
        /// <param name="name">Name of object to aniamte</param>
        /// <param name="time">Time of animation in miliseconds</param>
        UniTask StopSideUI(SteamPlayer sPlayer, ushort ID, short key, string name, int time);

        /// <summary>
        /// Stops all UIs for the player.
        /// </summary>
        /// <param name="sPlayer">SteamPlayer</param>
        /// <param name="useEffectManager">Does the method need to remove visuals</param>
        UniTask StopAllUI(SteamPlayer sPlayer, bool useEffectManager);

        /// <summary>
        /// Displays the warning on the player's UI.
        /// </summary>
        /// <param name="sPlayer">SteamPlayer</param>
        /// <param name="ID">Asset ID of UI</param>
        /// <param name="visibilityChildName">Name of gameobject that will be visible</param>
        /// <param name="textChildName">Name of gameobject with text script that can be edited</param>
        /// <param name="text">Text of warning that will be displayed</param>
        /// <param name="time">Duration of warning in seconds</param>
        UniTask DisplayWarning(SteamPlayer sPlayer, ushort ID, string visibilityChildName, string textChildName, string text, int time);

        /// <summary>
        /// Displays the error for the player.
        /// </summary>
        /// <param name="sPlayer">SteamPlayer</param>
        /// <param name="errorCode">Code of error</param>
        UniTask DisplayError(SteamPlayer sPlayer, byte errorCode);

        /// <summary>
        /// Closes the error for the player.
        /// </summary>
        /// <param name="sPlayer">SteamPlayer</param>
        /// <param name="useEffectManager">Does the method need to remove visuals</param>
        UniTask CloseError(SteamPlayer sPlayer, bool useEffectManager);

        /// <summary>
        /// Returns true if any error is displayed, false if not.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        bool IsErrorEnabled(CSteamID steamID);

        /// <summary>
        /// Returns true if selected warning is displayed, false if not.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <param name="visibilityChildName">Name of warning gameobject that is visible</param>
        bool IsWarningEnabled(CSteamID steamID, string visibilityChildName);

        /// <summary>
        /// Returns true if selected UI is enabled, false if not.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <param name="ID">Asset ID of UI</param>
        bool IsUIEnabled(CSteamID steamID, ushort ID);
    }
}
