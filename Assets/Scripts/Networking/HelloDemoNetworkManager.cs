using GameNetworking.Messages;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameNetworking
{
    public class HelloDemoNetworkManager : NetworkManager
    {
        const string GameplaySceneNameForDiFallback = "Main";

        [SerializeField] private TMP_Text _clientHelloText;

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
            _subscriptions?.ClientSubscribe<HelloMessage>(OnHelloMessageReceived);
            base.OnClientConnect();
        }

        private void OnHelloMessageReceived(HelloMessage msg)
        {
            Debug.Log(msg.Text);
            if (_clientHelloText != null)
                _clientHelloText.text = msg.Text;
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