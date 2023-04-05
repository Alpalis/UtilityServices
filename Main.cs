﻿using Alpalis.UtilityServices.API;
using Alpalis.UtilityServices.Events;
using Alpalis.UtilityServices.Models;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.Remoting.Lifetime;
using System.Threading.Tasks;
using System.Text.Json;
using Alpalis.UtilityServices.API.Github;
using Semver;
using Alpalis.UtilityServices.Services;

#region NuGet Assembly Data
[assembly:
    PluginMetadata("Alpalis.UtilityServices", Author = "Pandetthe",
        DisplayName = "Alpalis UtilityServices",
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
        private readonly IServiceProvider m_ServiceProvider;
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
            m_ServiceProvider = serviceProvider;
        }
        #endregion Class Constructor

        private CustomEventsListenersActivator? CustomEventsListenersActivator;

        protected override async UniTask OnLoadAsync()
        {
            // Version check
            await CheckGitHubNewerVersion();

            // Activate Custom Events Listeners
            CustomEventsListenersActivator = ActivatorUtilities.CreateInstance<CustomEventsListenersActivator>(m_ServiceProvider);
            CustomEventsListenersActivator.Activate();

            // Configuration load
            await m_ConfigurationManager.LoadConfigAsync<Config>(this);

            // Plugin Load Logging
            m_Logger.LogInformation("Plugin started successfully!");
        }

        protected override async UniTask OnUnloadAsync()
        {
            CustomEventsListenersActivator?.Dispose();

            // Event instance disposing
            m_KeyHandlerEvent.Dispose();

            // Plugin unload logging
            m_Logger.LogInformation("Plugin disabled successfully!");
        }

        private async Task CheckGitHubNewerVersion()
        {
            m_Logger.LogInformation("Checking version of UtilityServices...");
            using (HttpClient client = new())
            {
                client.BaseAddress = new("https://api.github.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.UserAgent.Add(new("UtilityServices", Version.ToString()));
                client.DefaultRequestHeaders.Accept.Add(new("application/vnd.github+json"));

                HttpResponseMessage response = await client.GetAsync("repos/Alpalis/UtilityServices/tags");
                if (response.IsSuccessStatusCode)
                {
                    List<Tag>? tags = JsonSerializer.Deserialize<List<Tag>>(await response.Content.ReadAsStringAsync());
                    if (tags == null || tags.Count == 0)
                    {
                        m_Logger.LogCritical("Alpalis UtilityServices plugin version check failed...");
                        return;
                    }
                    Tag tag = tags[0];
                    SemVersion githubVersion = SemVersion.FromVersion(new Version(tag.Name));
                    int result = githubVersion.ComparePrecedenceTo(Version);
                    if (result > 0)
                    {
                        m_Logger.LogCritical("Alpalis UtilityServices plugin is not up to date! " +
                            "Update the plugin as soon as possible, as there may be problems in the operation of other plugins. " +
                            "You can download the latest version here: https://github.com/Alpalis/UtilityServices/releases");
                    }
                    else if (result < 0)
                    {
                        m_Logger.LogInformation("Alpalis UtilityServices plugin version running on this server is higher than on github! " +
                            "You are doing a good job, keep it up man!");
                    }
                    else
                    {
                        m_Logger.LogInformation("You have the current version of the Alpalis UtilityServices plugin!");
                    }
                }
            }
        }
    }
}
