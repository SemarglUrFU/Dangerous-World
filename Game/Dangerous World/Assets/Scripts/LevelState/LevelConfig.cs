using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig_$", menuName = "Level/Config", order = 1)]
public class LevelConfig : ScriptableObject
{
    public string Id => _scene;
    public string Name => _name;
    public Sprite Preview => _preview;
    public int Cost => _cost;
    public int Lifes => _lifes;

#if UNITY_EDITOR
    [SerializeField] private SceneAsset _sceneAsset;
#endif
    [SerializeField] private string _scene;
    [SerializeField] private string _name;
    [SerializeField] private int _cost = 0;
    [SerializeField][Min(1)] private int _lifes = 3;
    [SerializeField] private Sprite _preview;

    [ContextMenu("Pass")]
    private void Pass0()
    {
        var state = LevelRepository.GetState(Id);
        state.Passed = true;
        state.Points = 0;
        LevelRepository.WriteState(Id, state);
    }

    [ContextMenu("Pass 1 points")]
    private void Pass1()
    {
        var state = LevelRepository.GetState(Id);
        state.Passed = true;
        state.Points = 1;
        LevelRepository.WriteState(Id, state);
    }

    [ContextMenu("Pass 3 points")]
    private void Pass3()
    {
        var state = LevelRepository.GetState(Id);
        state.Passed = true;
        state.Points = 3;
        LevelRepository.WriteState(Id, state);
    }

    [ContextMenu("Remove state")]
    private void Remove()
    {
        LevelRepository.Remove(Id);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        _scene = _sceneAsset.name;
#endif
        if (_name == "") {Debug.LogWarning("Empty scene name");}
    }

    public void __SetName(string name) => _name = name;
}
