using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.myProject.Scripts.Game.Gameplay.Root
{
    public class UIGameplayRootBinder : MonoBehaviour
    {
        public event Action GoToMainMenuButtonClicked;

        //TODO: check why Action missed when instantiate
        [SerializeField] private Button _buttonGoMenu;
        private void Awake()
        {
            _buttonGoMenu.onClick.AddListener(() => HandleGoToMainMenuButtonClick());
        }

        public void HandleGoToMainMenuButtonClick()
        {
            GoToMainMenuButtonClicked?.Invoke();
        }
    }
}