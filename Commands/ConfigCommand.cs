using Alpalis.UtilityServices.API;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.API.Plugins;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Plugins;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;
using System.Collections.Generic;

namespace Alpalis.UtilityServices.Commands
{
    [Command("config")]
    [CommandSyntax("<reload/display>")]
    [CommandDescription("Manage plugins' configs.")]
    public class ConfigRootCommand : UnturnedCommand
    {
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IAdminManagerImplementation m_AdminManagerImplementation;

        public ConfigRootCommand(
            IStringLocalizer stringLocalizer,
            IAdminManagerImplementation adminManagerImplementation,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_StringLocalizer = stringLocalizer;
            m_AdminManagerImplementation = adminManagerImplementation;
        }

        protected override async UniTask OnExecuteAsync()
        {
            if (!m_AdminManagerImplementation.IsInAdminMode(Context.Actor))
                throw new UserFriendlyException(m_StringLocalizer["reload_command:error_adminmode"]);
            throw new CommandWrongUsageException(Context);
        }
    }

    [Command("reload")]
    [CommandSyntax("[plugin full name]")]
    [CommandDescription("Reload plugins' configs.")]
    [CommandParent(typeof(ConfigRootCommand))]
    public class ConfigReloadCommand : UnturnedCommand
    {
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IConfigurationManager m_ConfigurationManager;
        private readonly IAdminManagerImplementation m_AdminManagerImplementation;

        public ConfigReloadCommand(
            IStringLocalizer stringLocalizer,
            IConfigurationManager configurationManager,
            IAdminManagerImplementation adminManagerImplementation,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_StringLocalizer = stringLocalizer;
            m_ConfigurationManager = configurationManager;
            m_AdminManagerImplementation = adminManagerImplementation;
        }

        protected override async UniTask OnExecuteAsync()
        {
            if (!m_AdminManagerImplementation.IsInAdminMode(Context.Actor))
                throw new UserFriendlyException(string.Format("{0}{1}",
                    Context.Actor is UnturnedUser ? m_StringLocalizer["config_command:prefix"] : "",
                    m_StringLocalizer["config_command:error_adminmode"]));
            if (Context.Parameters.Count == 0)
            {
                List<string> plugins = await m_ConfigurationManager.ReloadAllConfigsAsync();
                PrintAsync(string.Format("{0}{1}",
                    Context.Actor is UnturnedUser ? m_StringLocalizer["config_command:prefix"] : "",
                    m_StringLocalizer["config_command:reload:succeed_many:title"]));
                foreach (string plugin in plugins)
                {
                    await PrintAsync(m_StringLocalizer["config_command:reload:succeed_many:data",
                        new { PluginName = plugin }]);
                }
                return;
            }
            if (Context.Parameters.Count != 1)
                throw new CommandWrongUsageException(Context);
            if (!Context.Parameters.TryGet(0, out string? pluginName) || pluginName == null)
                throw new UserFriendlyException(string.Format("{0}{1}",
                    Context.Actor is UnturnedUser ? m_StringLocalizer["config_command:prefix"] : "",
                    m_StringLocalizer["config_command:error_null_pluginname"]));
            if (!await m_ConfigurationManager.ReloadConfigAsync(pluginName))
                throw new UserFriendlyException(string.Format("{0}{1}",
                    Context.Actor is UnturnedUser ? m_StringLocalizer["config_command:prefix"] : "",
                    m_StringLocalizer["config_command:error_unknown_plugin", new { PluginName = pluginName }]));
            PrintAsync(string.Format("{0}{1}",
                    Context.Actor is UnturnedUser ? m_StringLocalizer["config_command:prefix"] : "",
                    m_StringLocalizer["config_command:reload:succeed", new { PluginName = pluginName }]));
        }
    }


    [Command("display")]
    [CommandSyntax("[plugin full name]")]
    [CommandDescription("Display plugins' configs.")]
    [CommandParent(typeof(ConfigRootCommand))]
    public class ConfigDisplayCommand : UnturnedCommand
    {
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IConfigurationManager m_ConfigurationManager;
        private readonly IAdminManagerImplementation m_AdminManagerImplementation;
        private readonly IPluginActivator m_PluginActivator;

        public ConfigDisplayCommand(
            IStringLocalizer stringLocalizer,
            IConfigurationManager configurationManager,
            IAdminManagerImplementation adminManagerImplementation,
            IPluginActivator pluginActivator,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_StringLocalizer = stringLocalizer;
            m_ConfigurationManager = configurationManager;
            m_AdminManagerImplementation = adminManagerImplementation;
            m_PluginActivator = pluginActivator;
        }

        protected override async UniTask OnExecuteAsync()
        {
            if (Context.Parameters.Count > 1)
                throw new CommandWrongUsageException(Context);
            if (!m_AdminManagerImplementation.IsInAdminMode(Context.Actor))
                throw new UserFriendlyException(string.Format("{0}{1}",
                    Context.Actor is UnturnedUser ? m_StringLocalizer["config_command:prefix"] : "",
                    m_StringLocalizer["config_command:error_adminmode"]));
            if (Context.Parameters.Count == 0)
            {
                Dictionary<OpenModUnturnedPlugin, List<KeyValuePair<string, string>>> configs = m_ConfigurationManager.GetConfigProperties();
                await PrintAsync(string.Format("{0}{1}",
                    Context.Actor is UnturnedUser ? m_StringLocalizer["config_command:prefix"] : "",
                    m_StringLocalizer["config_command:display:succeed_many:title"]));
                foreach (KeyValuePair<OpenModUnturnedPlugin, List<KeyValuePair<string, string>>> pluginConfig in configs)
                {
                    await PrintAsync(m_StringLocalizer["config_command:display:succeed_many:plugin_title", new { PluginName = pluginConfig.Key.DisplayName }]);
                    foreach (KeyValuePair<string, string> property in pluginConfig.Value)
                    {
                        await PrintAsync(m_StringLocalizer["config_command:display:succeed_many:data",
                            new { Property = property.Key, property.Value }]);
                    }
                }
                return;
            }
            if (!Context.Parameters.TryGet(0, out string? pluginName) || pluginName == null)
                throw new UserFriendlyException(string.Format("{0}{1}",
                    Context.Actor is UnturnedUser ? m_StringLocalizer["config_command:prefix"] : "",
                    m_StringLocalizer["config_command:error_null_pluginname"]));
            OpenModUnturnedPlugin? plugin = m_PluginActivator.GetPluginBySimmilarName(pluginName);
            if (plugin == null)
                throw new UserFriendlyException(string.Format("{0}{1}",
                    Context.Actor is UnturnedUser ? m_StringLocalizer["config_command:prefix"] : "",
                    m_StringLocalizer["config_command:error_unknown_plugin", new { PluginName = pluginName }]));
            List<KeyValuePair<string, string>>? data = m_ConfigurationManager.GetConfigProperties(plugin);
            if (data == null)
                throw new UserFriendlyException(string.Format("{0}{1}",
                    Context.Actor is UnturnedUser ? m_StringLocalizer["config_command:prefix"] : "",
                    m_StringLocalizer["config_command:error_unknown_plugin", new { PluginName = pluginName }]));
            await PrintAsync(string.Format("{0}{1}",
                    Context.Actor is UnturnedUser ? m_StringLocalizer["config_command:prefix"] : "",
                    m_StringLocalizer["config_command:display:succeed:title", new { PluginName = plugin.DisplayName }]));
            foreach (KeyValuePair<string, string> property in data)
            {
                await PrintAsync(m_StringLocalizer["config_command:display:succeed:data",
                    new { Property = property.Key, property.Value }]);
            }
        }
    }
}
