using Alpalis.UtilityServices.API;
using Alpalis.UtilityServices.API.Events;
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Commands;
using OpenMod.API.Eventing;
using OpenMod.API.Ioc;
using OpenMod.API.Plugins;
using Steamworks;

namespace Alpalis.UtilityServices.Services
{
    [ServiceImplementation(Lifetime = ServiceLifetime.Transient)]
    public class AdminManagerImplementation : IAdminManagerImplementation
    {
        private readonly IEventBus m_EventBus;
        private readonly Main m_Plugin;

        public AdminManagerImplementation(
            IEventBus eventBus,
            IPluginAccessor<Main> plugin)
        {
            m_EventBus = eventBus;
            m_Plugin = plugin.Instance!;
        }

        public bool IsInAdminMode(CSteamID steamID)
        {
            AdminModeEvent @event = new(steamID);
            m_EventBus.EmitAsync(m_Plugin, this, @event);
            return @event.IsInAdminMode;
        }

        public bool IsInAdminMode(ICommandActor actor)
        {
            AdminModeEvent @event = new(actor);
            m_EventBus.EmitAsync(m_Plugin, this, @event);
            return @event.IsInAdminMode;
        }
    }
}
