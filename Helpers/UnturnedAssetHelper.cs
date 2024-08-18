using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;

namespace Alpalis.UtilityServices.Helpers
{
    /// <summary>
    /// A helper useful for finding game assets
    /// </summary>
    /// <remarks>
    /// <see href="https://github.com/Kr4ken-9/NewEssentials/blob/master/NewEssentials/Helpers/UnturnedAssetHelper.cs">
    /// Source code
    /// </see></remarks>
    public static class UnturnedAssetHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool GetItem(string searchTerm, out ItemAsset item)
        {
            if (string.IsNullOrEmpty(searchTerm.Trim()))
            {
                item = null!;
                return false;
            }

            if (!ushort.TryParse(searchTerm, out ushort id))
            {
                List<ItemAsset> items = [];
                Assets.find(items);
                item = items.Where(i => !string.IsNullOrEmpty(i.itemName))
                    .OrderBy(i => i.itemName.Length).FirstOrDefault(i =>
                        i.itemName.ToUpperInvariant().Contains(searchTerm.ToUpperInvariant()));

                return item != null;
            }
            item = (ItemAsset)Assets.find(EAssetType.ITEM, id);
            return item != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public static bool GetVehicle(string searchTerm, out VehicleAsset vehicle)
        {
            if (string.IsNullOrEmpty(searchTerm.Trim()))
            {
                vehicle = null!;
                return false;
            }

            if (!ushort.TryParse(searchTerm, out ushort id))
            {
                List<VehicleAsset> vehicles = [];
                Assets.find(vehicles);
                vehicle = vehicles.Where(v => !string.IsNullOrEmpty(v.vehicleName))
                    .OrderBy(v => v.vehicleName.Length).FirstOrDefault(v =>
                    v.vehicleName.ToUpperInvariant().Contains(searchTerm.ToUpperInvariant()));

                return vehicle != null;
            }

            vehicle = (VehicleAsset)Assets.find(EAssetType.VEHICLE, id);
            return vehicle != null;
        }
    }
}
