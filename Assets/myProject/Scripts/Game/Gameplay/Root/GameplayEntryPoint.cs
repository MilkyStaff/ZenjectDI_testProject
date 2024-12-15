using Assets.myProject.Scripts.Game.GameRoot;
using System;
using UnityEngine;

namespace Assets.myProject.Scripts.Game.Gameplay.Root
{
    public class  GameplayEntryPoint : MonoBehaviour
    {
        // MVVM => here we can get viewModel
        [SerializeField] private UIGameplayRootBinder _sceneUIRootBinderPrefab;

        //1
        public event Action GoToMainMenuSceneRequested;
        //

        //TODO: update to RX
        public void Run(UIRootView uiRoot)
        {
            var uiScene = Instantiate(_sceneUIRootBinderPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);
            
            //1
            uiScene.GoToMainMenuButtonClicked += () =>
            {
                GoToMainMenuSceneRequested?.Invoke();
            };
            //
        }
    }
}
