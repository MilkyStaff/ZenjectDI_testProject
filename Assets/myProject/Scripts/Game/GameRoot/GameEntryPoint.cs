using Assets.myProject.Scripts.Game.Gameplay.Root;
using Assets.myProject.Scripts.Game.GameRoot;
using Assets.myProject.Scripts.Game.MainMenu.Root;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            //set up CouroutinesHandler monobeh
            _couroutinesHandler = new GameObject("[COROUTINES]").AddComponent<CouroutinesHandler>();
            Object.DontDestroyOnLoad(_couroutinesHandler.gameObject);

            //set up UIRoot
            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            _uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_uiRoot.gameObject);

            //preload player Input, base container etc
        }

        private void RunGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            //base logic ?? no need duplicate
            switch (sceneName)
            {
                case SceneNames.BOOT:
                case SceneNames.MAIN_MENU:
                    _couroutinesHandler.StartCoroutine(LoadAndStartMainMenu());
                    return;

                case SceneNames.GAMEPLAY:
                    _couroutinesHandler.StartCoroutine(LoadAndStartGameplay());
                    return;
                default:
                    return;

            };
#endif

            _couroutinesHandler.StartCoroutine(LoadAndStartGameplay());
        }

        private void SetProjectSettings()
        {
            //Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private IEnumerator LoadAndStartGameplay()
        {
            _uiRoot.ShowLoadingScreen();

            yield return LoadScene(SceneNames.BOOT);
            yield return LoadScene(SceneNames.GAMEPLAY);
            
            //for check loadingScreen
            yield return new WaitForSeconds(2);
            //yield return null; // if GAMEPLAYscene is loaded by 1 frame

            var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
            sceneEntryPoint.Run(_uiRoot);

            //1
            sceneEntryPoint.GoToMainMenuSceneRequested += () =>
            {
                _couroutinesHandler.StartCoroutine(LoadAndStartMainMenu());
            };
            //

            _uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadAndStartMainMenu()
        {
            _uiRoot.ShowLoadingScreen();

            yield return LoadScene(SceneNames.BOOT);
            yield return LoadScene(SceneNames.MAIN_MENU);

            //for check loadingScreen
            yield return new WaitForSeconds(2);
            //yield return null; // if GAMEPLAYscene is loaded by 1 frame

            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            sceneEntryPoint.Run(_uiRoot);

            //1
            sceneEntryPoint.GoToGameplaySceneRequested += () =>
            {
                _couroutinesHandler.StartCoroutine(LoadAndStartGameplay());
            };
            //

            _uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
