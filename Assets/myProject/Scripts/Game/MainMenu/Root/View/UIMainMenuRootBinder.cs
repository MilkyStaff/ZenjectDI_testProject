using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.myProject.Scripts.Game.MainMenu.Root
{
    public class UIMainMenuRootBinder : MonoBehaviour
    {

        public event Action GoToGameplayButtonClicked;


        //TODO: check why Action missed when instantiate
        [SerializeField] private Button _buttonGoGameplay;
        private void Awake()
        {
            _buttonGoGameplay.onClick.AddListener(() => HandleGoToMainMenuButtonClick());
        }

        public void HandleGoToMainMenuButtonClick()
        {
            GoToGameplayButtonClicked?.Invoke();
        }
    }
}
