using Alpalis.UtilityServices.API;
using Alpalis.UtilityServices.Events;
using Alpalis.UtilityServices.Models;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;

#region NuGet Assembly Data
[assembly:
    PluginMetadata("Alpalis.UtilityServices", Author = "Pandetthe",
        DisplayName = "Alpalis UtilityServices Plugin",
        Website = "https://alpalis.eu")]
#endregion Nuget Assembly Data

namespace Alpalis.UtilityServices
{
    public class Main : OpenModUnturnedPlugin
    {
        #region Member Variables
        private readonly ILogger<Main> m_Logger;
        private readonly IDisposable m_KeyHandlerEvent;
        private readonly IConfigurationManager m_ConfigurationManager;
        #endregion Member Variables

        #region Class Constructor
        public Main(
            ILogger<Main> logger,
            IConfigurationManager configurationManager,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_Logger = logger;
            m_KeyHandlerEvent = ActivatorUtilities.CreateInstance<HandleKey>(serviceProvider);
            m_ConfigurationManager = configurationManager;
        }
        #endregion Class Constructor

        protected override async UniTask OnLoadAsync()
        {
            // Configuration load
            await m_ConfigurationManager.LoadConfig(this, new Config());

            // Plugin Load Logging
            m_Logger.LogInformation("Plugin started successfully!");
        }

        protected override async UniTask OnUnloadAsync()
        {
            // Event instance disposing
            m_KeyHandlerEvent.Dispose();

            // Plugin unload logging
            m_Logger.LogInformation("Plugin disabled successfully!");
        }
    }
}
