using OpenMod.API.Ioc;
using Steamworks;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.API
{
    /// <summary>
    /// Interface for managing player's UI input.
    /// </summary>
    [Service]
    public interface IUIHandler
    {
        /// <summary>
        /// Saves the text written in input.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <param name="inputName">Name of input</param>
        /// <param name="text">Text entered in input</param>
        Task SetText(CSteamID steamID, string inputName, string text);

        #region ClearText
        /// <summary>
        /// Clears all saved texts.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <returns></returns>
        Task ClearText(CSteamID steamID);

        /// <summary>
        /// Clears selected saved texts.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <param name="inputName">Name of input</param>
        Task ClearText(CSteamID steamID, string inputName);
        #endregion ClearText

        /// <summary>
        /// Returns true if the text of input is saved and false if not.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <param name="inputName">Name of input</param>
        /// <returns></returns>
        bool IsTextSet(CSteamID steamID, string inputName);

        /// <summary>
        /// Returns a bool array with values true if the text is saved and false if not.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <param name="inputName">Name of input</param>
        bool[] IsTextsSet(CSteamID steamID, string[] inputName);

        /// <summary>
        /// Returns true if the player has any text saved, false if not.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        bool IsPlayerSet(CSteamID steamID);

        /// <summary>
        /// Returns the saved text in input.
        /// </summary>
        /// <param name="steamID">SteamID of player</param>
        /// <param name="inputName">Name of input</param>
        string GetText(CSteamID steamID, string inputName);
    }
}
