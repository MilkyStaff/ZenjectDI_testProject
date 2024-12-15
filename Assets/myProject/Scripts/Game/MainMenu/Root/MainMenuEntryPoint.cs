using Assets.myProject.Scripts.Game.GameRoot;
using System;
using UnityEngine;

namespace Assets.myProject.Scripts.Game.MainMenu.Root
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        // MVVM => here we can get viewModel
        [SerializeField] private UIMainMenuRootBinder _sceneUIRootBinderPrefab;

        //1
        public event Action GoToGameplaySceneRequested;
        //

        //TODO: update to RX
        public void Run(UIRootView uiRoot)
        {
            var uiScene = Instantiate(_sceneUIRootBinderPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);

            //1
            uiScene.GoToGameplayButtonClicked += () =>
            {
                GoToGameplaySceneRequested?.Invoke();
            };
            //
        }
    }
}
