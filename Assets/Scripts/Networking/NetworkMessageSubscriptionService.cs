using System;
using GameNetworking.Messages;
using Mirror;
using UnityEngine;

namespace GameNetworking
{
    public class NetworkMessageSubscriptionService : INetworkMessageSubscriptionService
    {
        readonly NetworkMessageSubscriptionRegistry _registry = new NetworkMessageSubscriptionRegistry();

        bool _serverSubscribeHandlerRegistered;
        public event Action<NetworkConnectionToClient, ushort> ClientSubscribed;

        public void OnServerStarted()
        {
            if (_serverSubscribeHandlerRegistered)
                return;

            NetworkServer.RegisterHandler<ClientSubscribeNetworkMessage>(OnClientSubscribeMessage, false);
            _serverSubscribeHandlerRegistered = true;
        }

        public void OnServerStopped()
        {
            if (_serverSubscribeHandlerRegistered)
            {
                NetworkServer.UnregisterHandler<ClientSubscribeNetworkMessage>();
                _serverSubscribeHandlerRegistered = false;
            }

            _registry.Clear();
        }

        public void OnServerDisconnect(NetworkConnectionToClient connection)
        {
            _registry.RemoveConnection(connection);
        }

        public void ClientSubscribe<T>(Action<T> onMessage) where T : struct, NetworkMessage
        {
            NetworkClient.ReplaceHandler<T>(msg => onMessage?.Invoke(msg), false);

            ushort id = NetworkMessages.GetId<T>();
            NetworkClient.connection.Send(new ClientSubscribeNetworkMessage { MessageTypeId = id });
        }

        public bool TrySendToSubscribedClient<T>(NetworkConnectionToClient connection, T message)
            where T : struct, NetworkMessage
        {
            ushort id = NetworkMessages.GetId<T>();
            if (!_registry.IsSubscribed(connection, id))
                return false;

            connection.Send(message);
            return true;
        }

        void OnClientSubscribeMessage(NetworkConnectionToClient connection, ClientSubscribeNetworkMessage msg)
        {
            _registry.AddSubscription(connection, msg.MessageTypeId);
            ClientSubscribed?.Invoke(connection, msg.MessageTypeId);
        }
    }
}