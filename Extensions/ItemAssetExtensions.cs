using SDG.Unturned;

namespace Alpalis
{
    /// <summary>
    /// 
    /// </summary>
    public static class ItemAssetExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemAsset"></param>
        /// <returns></returns>
        public static string GetFullItemAssetName(this ItemAsset itemAsset)
        {
            return $"{itemAsset.name} ({itemAsset.id})";
        }
    }
}