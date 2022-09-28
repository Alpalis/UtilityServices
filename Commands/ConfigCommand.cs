using Alpalis.UtilityServices.API;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using System;
using System.Collections.Generic;

namespace Alpalis.UtilityServices.Commands
{
    #region Command Parameters
    [Command("config")]
    [CommandSyntax("<reload>")]
    [CommandDescription("Command to manage plugins' configs.")]
    #endregion Command Parameters
    public class ConfigRootCommand : UnturnedCommand
    {
        #region Member Variables
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IAdminManagerImplementation m_AdminManagerImplementation;
        #endregion Member Variables

        #region Class Constructor
        public ConfigRootCommand(
            IStringLocalizer stringLocalizer,
            IAdminManagerImplementation adminManagerImplementation,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_StringLocalizer = stringLocalizer;
            m_AdminManagerImplementation = adminManagerImplementation;
        }
        #endregion Class Constructor

        protected override UniTask OnExecuteAsync()
        {
            if (!m_AdminManagerImplementation.IsInAdminMode(Context.Actor))
                throw new UserFriendlyException(m_StringLocalizer["reload_command:error_adminmode"]);
            throw new CommandWrongUsageException(Context);
        }
    }

    #region Command Parameters
    [Command("reload")]
    [CommandSyntax("[plugin full name]")]
    [CommandDescription("Command to reload plugins' configs.")]
    [CommandParent(typeof(ConfigRootCommand))]
    #endregion Command Parameters
    public class ConfigReloadCommand : UnturnedCommand
    {
        #region Member Variables
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IConfigurationManager m_ConfigurationManager;
        private readonly IAdminManagerImplementation m_AdminManagerImplementation;
        #endregion Member Variables

        #region Class Constructor
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
        #endregion Class Constructor

        protected override async UniTask OnExecuteAsync()
        {
            if (!m_AdminManagerImplementation.IsInAdminMode(Context.Actor))
                throw new UserFriendlyException(m_StringLocalizer["reload_command:error_adminmode"]);
            if (Context.Parameters.Count == 0)
            {
                List<string> plugins = await m_ConfigurationManager.ReloadAllConfig();
                PrintAsync(m_StringLocalizer["config_command:succeed_many", new { Plugins = string.Join(", ", plugins) }]);
                return;
            }
            if (Context.Parameters.Count != 1)
                throw new CommandWrongUsageException(Context);
            if (!Context.Parameters.TryGet(0, out string? pluginName) || pluginName == null)
                throw new UserFriendlyException(m_StringLocalizer["config_command:error_null_pluginname"]);
            if (!await m_ConfigurationManager.ReloadConfig(pluginName))
                throw new UserFriendlyException(m_StringLocalizer["config_command:error_unknown_plugin", new { PluginName = pluginName}]);
            PrintAsync(m_StringLocalizer["config_command:succeed", new { PluginName = pluginName }]);
        }
    }
}
