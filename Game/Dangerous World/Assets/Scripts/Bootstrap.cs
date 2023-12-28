using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class Bootstrap : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset _loadSceneAsset;
    [SerializeField] private SceneAsset _loaderAsset;
#endif
    [SerializeField] private AudioComponent _audioComponent;
    [SerializeField] private GameObject _sceneTransition;
    [SerializeField] private string _loadScene;
    [SerializeField] private string _loader;


    private void Start()
    {
        _audioComponent.Initialize();
        DontDestroyOnLoad(_sceneTransition);
        SceneLoader.BindLoadScreen(_loader);
        SceneLoader.BindTransition(_sceneTransition.GetComponent<ISceneTransition>());
        SceneLoader.Load(_loadScene, SceneLoader.UseTransition.Out, false);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        _loadScene = _loadSceneAsset.name;
        _loader = _loaderAsset.name;
#endif
    }
}
