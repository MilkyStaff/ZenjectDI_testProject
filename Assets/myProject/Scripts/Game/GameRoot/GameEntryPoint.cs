using Assets.myProject.Scripts.Game.GameRoot;
using UnityEngine;

namespace Assets.myProject.Scripts
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;

        private CouroutinesHandler _couroutinesHandler;
        private UIRootView _uiRoot;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutoStartGame()
        {
            _instance = new();

            _instance.SetProjectSettings();
            _instance.RunGame();
        }

        private GameEntryPoint()
        {
            _couroutinesHandler = new GameObject("[COROUTINES]").AddComponent<CouroutinesHandler>();
            Object.DontDestroyOnLoad(_couroutinesHandler.gameObject);
        }

        private void RunGame()
        {
            
        }

        private void SetProjectSettings()
        {
            //Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
