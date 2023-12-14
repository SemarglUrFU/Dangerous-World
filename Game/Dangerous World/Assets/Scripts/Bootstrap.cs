using UnityEditor;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private SceneAsset _loadScene;
    [SerializeField] private SceneAsset _loader;
    [SerializeField] private GameObject _sceneTransition;

    void Start()
    {
        DontDestroyOnLoad(_sceneTransition);
        SceneLoader.BindLoadScreen(_loader.name);
        SceneLoader.BindTransition(_sceneTransition.GetComponent<ISceneTransition>());
        SceneLoader.Load(_loadScene.name, SceneLoader.UseTransition.Out, true);
    }
}
