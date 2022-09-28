using SDG.Unturned;

namespace Alpalis
{
    public static class ItemsExtensions
    {
        public static void ReverseClear(this Items item)
        {
            for (int b = item.getItemCount() - 1; b >= 0; b--)
            {
                item.removeItem((byte)b);
            }
        }
    }
}
