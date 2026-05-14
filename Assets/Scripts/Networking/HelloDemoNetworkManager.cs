using GameNetworking.Messages;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameNetworking
{
    public class HelloDemoNetworkManager : NetworkManager
    {
        const string GameplaySceneNameForDiFallback = "Main";

        INetworkMessageSubscriptionService _subscriptions;

        [Inject]
        void Construct(INetworkMessageSubscriptionService subscriptions)
        {
            _subscriptions = subscriptions;
        }

        public override void Start()
        {
            TryRegisterServerHandlers();
            base.Start();
        }

        public override void OnStartServer()
        {
            TryRegisterServerHandlers();
            base.OnStartServer();
        }

        public void TryRegisterServerHandlers()
        {
            if (!NetworkServer.active)
                return;

            _subscriptions.OnServerStarted();
        }

        void TryEnsureZenjectInjected()
        {
            if (_subscriptions != null)
                return;

            SceneContextRegistry registry = ProjectContext.Instance.Container.TryResolve<SceneContextRegistry>();
            if (registry == null)
                return;

            DiContainer container = registry.TryGetContainerForScene(gameObject.scene);
            if (container == null)
            {
                Scene gameplayScene = SceneManager.GetSceneByName(GameplaySceneNameForDiFallback);
                if (gameplayScene.IsValid())
                    container = registry.TryGetContainerForScene(gameplayScene);
            }

            container.InjectGameObject(gameObject);
        }

        public override void OnClientConnect()
        {
            _subscriptions?.ClientSubscribe<HelloMessage>(msg => Debug.Log(msg.Text));
            base.OnClientConnect();
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            _subscriptions?.OnServerStopped();
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            _subscriptions?.OnServerDisconnect(conn);
            base.OnServerDisconnect(conn);
        }
    }
}