using System;
using GameNetworking.Messages;
using Mirror;

namespace GameNetworking
{
    public interface INetworkMessageSubscriptionService
    {
        void OnServerStarted();
        void OnServerStopped();
        void OnServerDisconnect(NetworkConnectionToClient connection);
        void ClientSubscribe<T>(Action<T> onMessage) where T : struct, NetworkMessage;

        bool TrySendToSubscribedClient<T>(NetworkConnectionToClient connection, T message)
            where T : struct, NetworkMessage;
    }
}