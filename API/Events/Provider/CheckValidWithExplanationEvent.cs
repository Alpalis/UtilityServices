using OpenMod.Core.Eventing;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpalis.UtilityServices.API.Events.Provider
{
    public class CheckValidWithExplanationEvent : Event
    {
        public ValidateAuthTicketResponse_t Callback { get; set; }

        public bool IsValid { get; set; }

        public string Explanation { get; set; } = null!;

        public CheckValidWithExplanationEvent(ValidateAuthTicketResponse_t callback, bool isValid, string explanation)
        {
            Callback = callback;
            IsValid = isValid;
            Explanation = explanation;
        }
    }
}
