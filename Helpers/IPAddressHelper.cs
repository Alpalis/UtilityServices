using Steamworks;
using System.Net;

namespace Alpalis.UtilityServices.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class IPAddressHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static IPAddress GetIPAddressFromUInt(uint address)
        {
            return new(new byte[] {
                (byte)((address>>24) & 0xFF) ,
                (byte)((address>>16) & 0xFF) ,
                (byte)((address>>8)  & 0xFF) ,
                (byte)( address & 0xFF)});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static uint GetUIntFromIPAddress(IPAddress address)
        {
            byte[] bytes = address.GetAddressBytes();
            uint ipUInt = (uint)bytes[0] << 24;
            ipUInt += (uint)bytes[1] << 16;
            ipUInt += (uint)bytes[2] << 8;
            ipUInt += bytes[3];
            return ipUInt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static uint GetUIntFromSteamIPAddress(SteamIPAddress_t address)
        {
            // Also SteamIPAddress_tEx from SDG.Unturned can be used
            return GetUIntFromIPAddress(address.ToIPAddress());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static SteamIPAddress_t GetSteamIPAddressFromUint(uint address)
        {
            return new (GetIPAddressFromUInt(address));
        }
    }
}
