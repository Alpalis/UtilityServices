using Alpalis.UtilityServices.API;
using Alpalis.UtilityServices.Helpers;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using System;

namespace Alpalis.UtilityServices.Commands
{
    #region Command Parameters
    [Command("config")]
    [CommandSyntax("reload")]
    [CommandDescription("Command to manage plugins' configs.")]
    #endregion Command Parameters
    public class ConfigRootCommand : UnturnedCommand
    {
        #region Member Variables
        private readonly IStringLocalizer m_StringLocalizer;
        #endregion Member Variables

        #region Class Constructor
        public ConfigRootCommand(
            IStringLocalizer stringLocalizer,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_StringLocalizer = stringLocalizer;
        }
        #endregion Class Constructor

        protected override UniTask OnExecuteAsync()
        {
            if (!AdminModeHelper.IsInAdminMode(Context.Actor))
                throw new UserFriendlyException(m_StringLocalizer["reload_command:error_admin_mode"]);
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
        #endregion Member Variables

        #region Class Constructor
        public ConfigReloadCommand(
            IStringLocalizer stringLocalizer,
            IConfigurationManager configurationManager,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_StringLocalizer = stringLocalizer;
            m_ConfigurationManager = configurationManager;
        }
        #endregion Class Constructor

        protected override async UniTask OnExecuteAsync()
        {
            if (!AdminModeHelper.IsInAdminMode(Context.Actor))
                throw new UserFriendlyException(m_StringLocalizer["reload_command:error_admin_mode"]);
            if (Context.Parameters.Count == 0)
            {
                m_ConfigurationManager.ReloadAllConfig();
                PrintAsync("");
                return;
            }
            if (Context.Parameters.Count != 1)
                throw new CommandWrongUsageException(Context);
            if (!Context.Parameters.TryGet(0, out string? pluginName) || pluginName == null)
                throw new UserFriendlyException(m_StringLocalizer["company_command:error_null_pluginname"]);

        }
    }
}
