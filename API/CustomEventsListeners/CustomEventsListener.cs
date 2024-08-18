using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Eventing;
using OpenMod.Core.Helpers;
using System;

namespace Alpalis.UtilityServices.API.CustomEventsListeners
{
    internal abstract class CustomEventsListener(IServiceProvider serviceProvider) : ICustomEventsListener
    {
        private readonly Main m_Plugin = serviceProvider.GetRequiredService<Main>();

        private readonly IEventBus m_EventBus = serviceProvider.GetRequiredService<IEventBus>();

        protected void Emit(IEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));
            AsyncHelper.RunSync(() => m_EventBus.EmitAsync(m_Plugin, this, @event));
        }

        public abstract void Subscribe();

        public abstract void Unsubscribe();
    }
}
