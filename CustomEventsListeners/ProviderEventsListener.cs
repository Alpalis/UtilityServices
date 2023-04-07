using Alpalis.UtilityServices.API.CustomEventsListeners;
using Alpalis.UtilityServices.API.Events.Provider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SDG.Unturned;
using Steamworks;
using System;

namespace Alpalis.UtilityServices.EventsListeners
{
    internal class ProviderEventsListener : CustomEventsListener
    {
        private readonly ILogger<ProviderEventsListener> _logger;

        public ProviderEventsListener(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<ProviderEventsListener>>();
        }

        public override void Subscribe()
        {
            /* 
             * IMPLEMENTED IN OPENMOD
             * onBanPlayerRequestedV2 - UnturnedPlayerBanningEvent
             * onCheckBanStatusWithHWID - UnturnedPlayerCheckingBanEvent
             * onUnbanPlayerRequested - UnturnedPlayerUnbanningEvent, UnturnedPlayerUnbannedEvent
             * onServerConnected - UnturnedPlayerConnectedEvent
             * onServerDisconnected - UnturnedPlayerDisconnectedEvent
             * onCommenceShutdown  - UnturnedShutdownCommencedEvent
             * 
             * NOT TO PLUGIN USAGE / NOT WORKING
             * onQueuePositionUpdated - client side
             * onEnemyConnected
             * onEnemyDisconnected
             * onClientConnected - client side
             * onClientDisconnected - client side
             * 
             * UNKNOWN
             * onBackendRealtimeAvailable
             * onLoginSpawning
             * onServerShutdown
             */

            Provider.onRejectingPlayer += OnRejectingPlayer;
            Provider.onBattlEyeKick += OnBattlEyeKick;
            Provider.onServerHosted += OnServerHosted;
            Provider.onCheckValidWithExplanation += OnCheckValidWithExplanation;
        }
        private void OnRejectingPlayer(CSteamID steamID, ESteamRejection rejection, string explanation)
        {
            RejectingPlayerEvent @event = new(steamID, rejection, explanation);
            Emit(@event);
        }

        private void OnBattlEyeKick(SteamPlayer client, string reason)
        {
            BattleEyeKickedEvent @event = new(client, reason);
            Emit(@event);
        }

        private void OnServerHosted()
        {
            ServerHostedEvent @event = new();
            Emit(@event);
        }

        private void OnCheckValidWithExplanation(ValidateAuthTicketResponse_t callback, ref bool isValid, ref string explanation)
        {
            CheckValidWithExplanationEvent @event = new(callback, isValid, explanation);
            Emit(@event);
            isValid = @event.IsValid;
            explanation = @event.Explanation;
        }

        public override void Unsubscribe()
        {
            Provider.onRejectingPlayer -= OnRejectingPlayer;
            Provider.onBattlEyeKick -= OnBattlEyeKick;
            Provider.onServerHosted -= OnServerHosted;
            Provider.onCheckValidWithExplanation -= OnCheckValidWithExplanation;
        }
    }
}
