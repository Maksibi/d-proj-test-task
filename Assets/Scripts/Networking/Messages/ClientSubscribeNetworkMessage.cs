using Mirror;

namespace GameNetworking.Messages
{
    public struct ClientSubscribeNetworkMessage : NetworkMessage
    {
        public ushort MessageTypeId;
    }
}