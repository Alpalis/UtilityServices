using System;
using System.Collections.Generic;

namespace Alpalis.UtilityServices.Models
{
    public abstract class MainConfig
    {
        public abstract List<KeyValuePair<string, string>> GetPropertiesInString();

        public virtual bool IsValid(out List<string> errors)
        {
            errors = new();
            return true;
        }
    }
}
