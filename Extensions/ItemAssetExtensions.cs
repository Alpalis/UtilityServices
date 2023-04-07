using SDG.Unturned;

namespace Alpalis
{
    public static class ItemAssetExtensions
    {
        public static string GetFullItemAssetName(this ItemAsset itemAsset)
        {
            return $"{itemAsset.name} ({itemAsset.id})";
        }
    }
}