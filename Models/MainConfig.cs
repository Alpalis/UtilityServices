using System;
using System.Collections.Generic;

namespace Alpalis.UtilityServices.Models
{
    public abstract class MainConfig
    {
        public abstract List<KeyValuePair<string, string>> GetPropertiesInString();
    }
}
