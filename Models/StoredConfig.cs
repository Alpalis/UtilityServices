using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.Models
{
    public class StoredConfig
    {
        public StoredConfig(MainConfig config, Type type)
        {
            Config = config;
            Type = type;
        }

        public MainConfig Config { get; set; }

        public Type Type { get; set; }
    }
}
