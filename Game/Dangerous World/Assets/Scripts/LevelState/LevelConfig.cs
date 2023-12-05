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
}
