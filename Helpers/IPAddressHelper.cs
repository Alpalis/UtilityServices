using System.Net;

namespace Alpalis.UtilityServices.Helpers
{
    public static class IPAddressHelper
    {
        public static IPAddress GetIPAddressFromUInt(uint address)
        {
            return new(new byte[] {
                (byte)((address>>24) & 0xFF) ,
                (byte)((address>>16) & 0xFF) ,
                (byte)((address>>8)  & 0xFF) ,
                (byte)( address & 0xFF)});
        }

        public static uint GetUIntFromIPAddress(IPAddress address)
        {
            byte[] bytes = address.GetAddressBytes();
            uint ipUInt = (uint)bytes[0] << 24;
            ipUInt += (uint)bytes[1] << 16;
            ipUInt += (uint)bytes[2] << 8;
            ipUInt += bytes[3];
            return ipUInt;
        }
    }
}
