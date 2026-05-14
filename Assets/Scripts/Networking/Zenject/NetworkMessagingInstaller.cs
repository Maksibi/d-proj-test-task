using Zenject;
using UnityEngine;

namespace GameNetworking
{
    public class NetworkMessagingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NetworkMessageSubscriptionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<HelloDemoSubscriptionResponder>().AsSingle().NonLazy();
        }
    }
}