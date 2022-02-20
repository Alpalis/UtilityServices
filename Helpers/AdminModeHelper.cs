#pragma warning disable IDE0060

using OpenMod.API.Commands;

namespace Alpalis.UtilityServices.Helpers
{
    public static class AdminModeHelper
    {
        public static bool IsInAdminMode(ICommandActor actor)
        {
            return true;
        }
    }
}
