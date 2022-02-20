using Alpalis.UtilityServices.Models;
using OpenMod.API.Ioc;
using OpenMod.Unturned.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.API
{
    /// <summary>
    /// 
    /// </summary>
    [Service]
    public interface IConfigurationManager
    {
        Task LoadConfig(OpenModUnturnedPlugin plugin, MainConfig config);

        Task ReloadAllConfig();

        T GetConfig<T>(OpenModUnturnedPlugin plugin);
    }
}
