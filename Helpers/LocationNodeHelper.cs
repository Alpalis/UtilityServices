using SDG.Unturned;
using System.Linq;

namespace Alpalis.UtilityServices.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class LocationNodeHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        public static LocationNode GetLocationNode(string place)
        {
            return (
                from node in LevelNodes.nodes
                where node.type == ENodeType.LOCATION
                let locNode = node as LocationNode
                where locNode.name.ToLower().Contains(place.ToLower())
                select locNode
            ).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="place"></param>
        /// <param name="locationNode"></param>
        /// <returns></returns>
        public static bool TryGetLocationNode(string place, out LocationNode locationNode)
        {
            locationNode = GetLocationNode(place);
            return locationNode != null;
        }
    }
}
