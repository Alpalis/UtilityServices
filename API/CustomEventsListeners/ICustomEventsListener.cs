namespace Alpalis.UtilityServices.API.CustomEventsListeners
{
    internal interface ICustomEventsListener
    {
        void Subscribe();

        void Unsubscribe();
    }
}
