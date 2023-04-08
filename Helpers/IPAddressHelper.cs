

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
    }
}
