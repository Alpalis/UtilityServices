using System;
using System.Collections.Generic;

namespace Alpalis.UtilityServices.Models
{
    [Serializable]
    public class Config : MainConfig
    {
        #region Class Constructor
        public Config()
        {
            AutomaticConfigReload = false;
        }
        #endregion Class Constructor

        public bool AutomaticConfigReload { get; set; }

        public override List<KeyValuePair<string, string>> GetPropertiesInString()
        {
            return new List<KeyValuePair<string, string>>()
            {
                new ("AutomaticConfigReload", $"{AutomaticConfigReload}")
            };
        }
    }
}
