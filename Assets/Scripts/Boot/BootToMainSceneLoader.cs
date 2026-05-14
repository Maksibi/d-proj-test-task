using UnityEngine.SceneManagement;
using Zenject;

namespace Game.Boot
{
    public class BootToMainSceneLoader : IInitializable
    {
        private readonly string _mainSceneName;

        public BootToMainSceneLoader(string mainSceneName)
        {
            _mainSceneName = mainSceneName;
        }

        public void Initialize()
        {
            SceneManager.LoadScene(_mainSceneName, LoadSceneMode.Single);
        }
    }
}