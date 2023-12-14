using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig_$", menuName = "Level/Config", order = 1)]
public class LevelConfig : ScriptableObject
{
    public string Id => _scene.name;
    public string Scene => _scene.name;
    public Sprite Preview => _preview;
    public int Cost => _cost;

    [SerializeField] private SceneAsset _scene;
    [SerializeField] private int _cost = 0;
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
}
