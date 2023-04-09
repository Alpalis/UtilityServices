using Alpalis.UtilityServices.API.CustomEventsListeners;
using Autofac;
using Microsoft.Extensions.Logging;
using OpenMod.Common.Helpers;
using OpenMod.Core.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alpalis.UtilityServices.CustomEventsListeners
{
    /// <summary>
    /// Events testing before adding to openmod
    /// </summary>
    internal class CustomEventsListenersActivator : IDisposable
    {
        private readonly ILifetimeScope m_LifetimeScope;
        private readonly ILogger<CustomEventsListenersActivator> m_Logger;
        private readonly List<ICustomEventsListener> m_UnturnedEventsListeners;

        public CustomEventsListenersActivator(
            ILifetimeScope lifetimeScope,
            ILogger<CustomEventsListenersActivator> logger)
        {
            m_LifetimeScope = lifetimeScope;
            m_Logger = logger;
            m_UnturnedEventsListeners = new();
        }

        public void Activate()
        {
            m_Logger.LogTrace("Activating custom events listeners");

            List<Type>? listenerTypes = GetType().Assembly.FindTypes<ICustomEventsListener>(false).ToList();
            foreach (Type type in listenerTypes)
            {
                ICustomEventsListener eventsListener = (ICustomEventsListener)ActivatorUtilitiesEx.CreateInstance(m_LifetimeScope, type);
                m_UnturnedEventsListeners.Add(eventsListener);
            }

            foreach (ICustomEventsListener eventsListener in m_UnturnedEventsListeners)
            {
                eventsListener.Subscribe();
            }
        }

        public void Dispose()
        {
            m_Logger.LogTrace("Disposing custom events listeners");

            foreach (ICustomEventsListener eventsListener in m_UnturnedEventsListeners)
            {
                eventsListener.Unsubscribe();
            }

            m_UnturnedEventsListeners.Clear();
        }
    }
}
