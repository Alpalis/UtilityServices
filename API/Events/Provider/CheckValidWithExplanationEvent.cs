using OpenMod.Core.Eventing;
using Steamworks;

namespace Alpalis.UtilityServices.API.Events.Provider
{
    public class CheckValidWithExplanationEvent(ValidateAuthTicketResponse_t callback, bool isValid, string explanation) : Event
    {
        public ValidateAuthTicketResponse_t Callback { get; } = callback;

        public bool IsValid { get; } = isValid;

        public string Explanation { get; } = explanation;
    }
}
