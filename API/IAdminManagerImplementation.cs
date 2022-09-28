using Alpalis.UtilityServices.Models;
using OpenMod.API.Commands;
using OpenMod.API.Ioc;
using OpenMod.Unturned.Plugins;
using Steamworks;

namespace Alpalis.UtilityServices.API
{
    /// <summary>
    /// Interface implementing AdminManager plugin.
    /// </summary>
    [Service]
    public interface IAdminManagerImplementation
    {
        #region IsInAdminMode
        /// <summary>
        /// Checks if the player is in admin mode.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <returns>Returns true if the player is in admin mode and false if not.</returns>
        bool IsInAdminMode(CSteamID steamID);

        /// <summary>
        /// Checks if the player is in admin mode.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <returns>Returns true if the player is in admin mode and false if not.</returns>
        bool IsInAdminMode(ICommandActor actor);
        #endregion IsInAdminMode
    }
}
