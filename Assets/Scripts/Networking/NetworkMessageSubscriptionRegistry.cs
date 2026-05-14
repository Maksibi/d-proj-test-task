using System.Collections.Generic;
using Mirror;

namespace GameNetworking
{
    public class NetworkMessageSubscriptionRegistry
    {
        readonly Dictionary<NetworkConnectionToClient, HashSet<ushort>> _byConnection =
            new Dictionary<NetworkConnectionToClient, HashSet<ushort>>();

        public void AddSubscription(NetworkConnectionToClient connection, ushort messageTypeId)
        {
            if (!_byConnection.TryGetValue(connection, out HashSet<ushort> set))
            {
                set = new HashSet<ushort>();
                _byConnection[connection] = set;
            }

            set.Add(messageTypeId);
        }

        public bool IsSubscribed(NetworkConnectionToClient connection, ushort messageTypeId)
        {
            return _byConnection.TryGetValue(connection, out HashSet<ushort> set) && set.Contains(messageTypeId);
        }

        public void RemoveConnection(NetworkConnectionToClient connection)
        {
            _byConnection.Remove(connection);
        }

        public void Clear()
        {
            _byConnection.Clear();
        }
    }
}
