using System;
using UnityEngine;

namespace Assets.myProject.Scripts.Game.GameRoot
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Transform _uiSceneContainer;

        private void Awake()
        {
            HideLoadingScreen();
        }

        public void ShowLoadingScreen()
        {
            _loadingScreen.SetActive(true);
        }
        public void HideLoadingScreen()
        {
            _loadingScreen.SetActive(false);
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            CleanSceneUI();
            sceneUI.transform.SetParent(_uiSceneContainer, false);
        }

        private void CleanSceneUI()
        {
            //for (int i = _uiSceneContainer.childCount - 1; i >= 0; i--)
            for (int i = 0; i < _uiSceneContainer.childCount; i++)
                Destroy(_uiSceneContainer.GetChild(i).gameObject);
        }
    }
}
