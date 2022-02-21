using OpenMod.API.Ioc;
using Steamworks;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.API
{
    /// <summary>
    /// Interface for managing player's key input.
    /// </summary>
    [Service]
    public interface IKeyHandler
    {
        /// <summary>
        /// Sets the state of the button as pressed.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <param name="key">ID of key</param>
        Task SetKeyDown(CSteamID steamID, int key);

        /// <summary>
        /// Sets the state of the button as released.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <param name="key">ID of key</param>
        Task SetKeyUp(CSteamID steamID, int key);

        /// <summary>
        /// Returns true if the button is pressed and false if not.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <param name="key">ID of key</param>
        bool GetKeyState(CSteamID steamID, int key);

        /// <summary>
        /// Returns true if the cooldown has passed, and false if not.
        /// </summary>
        /// <param name="steamID">>SteamID of player</param>
        /// <param name="key">ID of key</param>
        bool GetKeyCooldown(CSteamID steamID, int key);
    }
}
