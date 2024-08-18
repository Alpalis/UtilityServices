using SDG.Framework.Devkit;
using SDG.Unturned;
using System;
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
        public static LocationDevkitNode? GetLocationNode(string place)
        {
            return (LocationDevkitNode)LevelHierarchy.instance.items.Where(i => i is LocationDevkitNode location &&
                location.name.Contains(place, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="place"></param>
        /// <param name="locationNode"></param>
        /// <returns></returns>
        public static bool TryGetLocationNode(string place, out LocationDevkitNode? locationNode)
        {
            locationNode = GetLocationNode(place);
            return locationNode != null;
        }
    }
}
