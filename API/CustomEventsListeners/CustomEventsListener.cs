using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Eventing;
using OpenMod.API.Users;
using OpenMod.API;
using OpenMod.Core.Helpers;
using OpenMod.Unturned.Players;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Alpalis.UtilityServices.API.CustomEventsListeners
{
    internal abstract class CustomEventsListener : ICustomEventsListener
    {
        private readonly Main m_Plugin;
        private readonly IEventBus m_EventBus;

        protected CustomEventsListener(IServiceProvider serviceProvider)
        {
            m_Plugin = serviceProvider.GetRequiredService<Main>();
            m_EventBus = serviceProvider.GetRequiredService<IEventBus>();
        }

        protected void Emit(IEvent @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            AsyncHelper.RunSync(() => m_EventBus.EmitAsync(m_Plugin, this, @event));
        }

        public abstract void Subscribe();

        public abstract void Unsubscribe();
    }
}
