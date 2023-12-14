using System;
using UnityEngine;
using UnityEngine.SceneManagement;

static class SceneLoader
{
    private static ISceneTransition _sceneTransition;
    private static string _loadScreenName;

    public static Action OnSceneLoad;
    public static Action OnSceneExit;

    public static void Load(string scene, UseTransition transition, bool useLoadScreen = false)
    {
        var (useIn, useOut) = transition == UseTransition.Both 
            ? (true, true) : (transition == UseTransition.In, transition == UseTransition.Out);
        
        if (useIn)
        {
            _sceneTransition.PlayIn();
            _sceneTransition.OnCurrentAnimationEnded.AddListener(StartLoadScene);
        }
        else { StartLoadScene(); }

        void StartLoadScene()
        {
            OnSceneExit?.Invoke();
            if (useLoadScreen)
            {
                SceneManager.LoadScene(_loadScreenName);
                SceneManager.sceneLoaded += OnLoaderLoad;
            }
            else { LoadSceneAsync(); }
        }

        void OnLoaderLoad(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnLoaderLoad;
            _sceneTransition.Hide();
            LoadSceneAsync();
        }

        void LoadSceneAsync()
        {
            var loadOperation = SceneManager.LoadSceneAsync(scene);
            loadOperation.allowSceneActivation = true;
            loadOperation.completed += OnSceneAsyncLoad;
        }

        void OnSceneAsyncLoad(AsyncOperation operation)
        {
            OnSceneLoad?.Invoke();
            if (useOut) { _sceneTransition.PlayOut(); }
        }
    }

    public static void BindTransition(ISceneTransition sceneTransition) => _sceneTransition = sceneTransition;
    public static void BindLoadScreen(string loadScreenName) => _loadScreenName = loadScreenName;
    public enum UseTransition:byte{None, In, Out, Both}
}