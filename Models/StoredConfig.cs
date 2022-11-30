using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Alpalis.UtilityServices.Models
{
    public class StoredConfig
    {
        public StoredConfig(MainConfig config, Type type, IDisposable changeListener)
        {
            Config = config;
            Type = type;
            ChangeListener = changeListener;
        }

        public MainConfig Config { get; set; }

        public Type Type { get; set; }

        public IDisposable ChangeListener { get; set; }
    }
}
