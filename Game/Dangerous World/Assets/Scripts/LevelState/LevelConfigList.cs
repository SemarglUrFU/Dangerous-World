using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.ObjectModel;

[CreateAssetMenu(fileName = "LevelList", menuName = "Level/List", order = 1)]
public class LevelConfigList : ScriptableObject
{
    public ReadOnlyCollection<LevelConfig> Levels => _levels.AsReadOnly();
    public int Count => _levels.Count;
    public LevelConfig GetLevel(string id) => _levels.Find((level) => level.Id == id);
    [SerializeField] private List<LevelConfig> _levels = new();

    private void OnValidate()
    {
        if (_levels.Count > 1 && _levels
            .GroupBy((level) => level.Id)
            .Any((group) => group.Count() > 1))
        {
            Debug.LogError("Non unique levels IDs");
        }
    }
}
