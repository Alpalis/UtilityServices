using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.API.CustomEventsListeners
{
    internal interface ICustomEventsListener
    {
        void Subscribe();

        void Unsubscribe();
    }
}
