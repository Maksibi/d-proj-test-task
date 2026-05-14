using UnityEngine;
using Zenject;

namespace Game.Boot
{
    public class BootInstaller : MonoInstaller
    {
        [SerializeField] private string _mainSceneName = "Main";

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BootToMainSceneLoader>()
                .AsSingle()
                .WithArguments(_mainSceneName)
                .NonLazy();
        }
    }
}