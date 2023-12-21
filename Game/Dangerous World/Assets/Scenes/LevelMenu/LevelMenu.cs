using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] private LevelConfigList _levelConfigList;
    [Header("UI")]
    [SerializeField] private TMP_Text _playerStars;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _prevButton;
    [SerializeField] private LevelMenuState _levelMenuState;
    [SerializeField] private LevelMenuIndicator _levelMenuIndicator;

    private int _selectedLevel = 0;

    private void Awake()
    {
        _selectedLevel = Prefs.SelectedLevel;
    }

    private void Start()
    {
        _playerStars.text = Prefs.Points.ToString();
        _levelMenuIndicator.Initialize(_levelConfigList.Count);
        TrySelectLevel(_selectedLevel);
    }

    public void TrySelectNeighborLevel(int delta) => TrySelectLevel(_selectedLevel + delta);

    public void TrySelectLevel(int index)
    {
        index = Math.Clamp(index, 0, _levelConfigList.Count - 1);
        SelectLevel(index);
    }

    public void TryLoadSelectedLevel()
    {
        if (!IsLevelUnlocked(_selectedLevel)) { return; }
        LoadLevel(_selectedLevel);
    }

    private void SelectLevel(int index)
    {
        var levelConfig = _levelConfigList.Levels[index];
        var levelState = LevelRepository.GetState(levelConfig.Id);
        UpdateButtonsState(index);
        _levelMenuIndicator.UpdateSelectedLevel(index);
        _levelMenuState.UpdateVisual(levelConfig.name, levelConfig.Preview, IsLevelUnlocked(index), levelState.Points, levelConfig.Cost);
        _selectedLevel = index;
    }

    private void LoadLevel(int index)
    {
        LevelLoader.LoadLevel(new(_levelConfigList, index));
    }

    private void UpdateButtonsState(int index)
    {
        var (next, prev) = (true, true);
        _nextButton.gameObject.SetActive(next);
        if (index == 0) prev = false;
        if (index == _levelConfigList.Count - 1) next = false;
        _nextButton.gameObject.SetActive(next);
        _prevButton.gameObject.SetActive(prev);
    }

    private bool IsPreviousLevelPassed(int index)
    {
        return (index == 0) || LevelRepository.GetState(_levelConfigList.Levels[index - 1].Id).Passed;
    }

    private bool IsLevelUnlocked(int index)
    {
        var levelConfig = _levelConfigList.Levels[index];
        return IsPreviousLevelPassed(index)
            && (levelConfig.Cost == 0 || Prefs.Points >= levelConfig.Cost);
    }
}
