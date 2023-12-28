using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "Level/List", order = 1)]
public class LevelConfigList : ScriptableObject
{
    public ReadOnlyCollection<LevelConfig> Levels => _levels.AsReadOnly();
    public int Count => _levels.Count;
    public LevelConfig GetLevel(string id) => _levels.Find((level) => level.Id == id);
    [SerializeField] private List<LevelConfig> _levels = new();

    private void OnValidate()
    {
        if (_levels.Count == 0)
            throw new Exception("List must contain at least one level");
        if (_levels.Count > 1 && _levels
            .GroupBy((level) => level.Id)
            .Any((group) => group.Count() > 1))
        {
            Debug.LogError("Non unique levels IDs");
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Enumerate Levels")]
    private void Enumerate()
    {
        for (var i = 0; i < _levels.Count; i++) { if (_levels[i].Name == "") { _levels[i].__SetName((i + 1).ToString()); } }
    }
    [ContextMenu("Remove all states")]
    private void RemoveAllStates()
    {
        _levels.ForEach(level => LevelRepository.Remove(level.Id));
    }

    [ContextMenu("Add player 5 Points")]
    private void AddPlayerPoints()
    {
        Prefs.AddPoints(5);
    }

    [ContextMenu("Reset player Point")]
    private void ResetPlayerPoint()
    {
        Prefs.__ResetPoints();
    }
#endif
}
