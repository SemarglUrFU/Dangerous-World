using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FirstMenuBootstrap : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset _nextAsset;
#endif
    [SerializeField] private Button _startButton;
    [SerializeField] private string _next;

    private void Start()
    {
        _startButton.onClick.AddListener(
            () => SceneLoader.Load(_next, SceneLoader.UseTransition.Both, true));
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        _next = _nextAsset.name;
#endif
    }
}
