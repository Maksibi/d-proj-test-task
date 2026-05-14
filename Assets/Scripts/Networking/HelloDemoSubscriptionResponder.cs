using System;
using GameNetworking.Messages;
using Mirror;
using Zenject;

namespace GameNetworking
{
    public class HelloDemoSubscriptionResponder : IDisposable
    {
        readonly NetworkMessageSubscriptionService _service;

        public HelloDemoSubscriptionResponder(NetworkMessageSubscriptionService service) 
        {
            _service = service;
            _service.ClientSubscribed += OnClientSubscribed;
        }

        public void Dispose()
        {
            _service.ClientSubscribed -= OnClientSubscribed;
        }

        void OnClientSubscribed(NetworkConnectionToClient connection, ushort messageTypeId)
        {
            if (messageTypeId != NetworkMessages.GetId<HelloMessage>())
                return;

            _service.TrySendToSubscribedClient(connection, new HelloMessage { Text = "Hello Client!" });
        }
    }
}