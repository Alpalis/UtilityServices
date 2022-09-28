using OpenMod.API.Ioc;
using Steamworks;

namespace Alpalis.UtilityServices.API
{
    /// <summary>
    /// Interface implementing IdentityManager plugin.
    /// </summary>
    [Service]
    public interface IIdentityManagerImplementation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="steamID"></param>
        /// <returns></returns>
        ushort? GetIdentity(CSteamID steamID);
    }
}
