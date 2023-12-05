using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public static readonly string SELECTED_LEVEL_KEY = "_SL";
    
    [Header("Levels")]
    [SerializeField] private LevelConfigList _levelConfigList;
    [Header("UI")]
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _prevButton;
    [SerializeField] private TextMeshPro _levelName;
    [SerializeField] private Image _preview;
    [SerializeField] private LevelMenuLock _levelMenuLock;
    [SerializeField] private LevelMenuStars _levelMenuStars;
    [SerializeField] private LevelMenuIndicator _levelMenuIndicator;

    private int _selectedLevel = 0;
    private LevelState _levelState = null;

    private void Awake()
    {
        if (!Repository.TryReadValue(SELECTED_LEVEL_KEY, out _selectedLevel))
            _selectedLevel = 0;
    }

    private void Start()
    {
        SelectLevel(_selectedLevel);
        // !
        LevelLoader.LoadLevel(new(_levelConfigList, _selectedLevel));
        // !
    }

    public void TrySelectNeighborLevel(int delta) => TrySelectLevel(_selectedLevel + delta);

    public void TrySelectLevel(int index)
    {
        if (index < 0)
        {
            index = 0;
            SetButtonState(_prevButton, false);
        }
        else if (index > _levelConfigList.Count - 1)
        {
            index = _levelConfigList.Count - 1;
            SetButtonState(_nextButton, false);
        }
        else 
        {
            SetButtonState(_prevButton, true);
            SetButtonState(_nextButton, true);
        }
        SelectLevel(index);
    }

    private void SelectLevel(int index)
    {
        var levelConfig = _levelConfigList.Levels[index];
        _levelState = LevelRepository.GetState(levelConfig.Id);

        // _preview.sprite = levelConfig.Preview;
        // _levelName.text = $"Уровень {index + 1}";
        // _levelMenuStars.Set(_levelState.Points);
        // _levelMenuLock.UpdateSelf(_levelState.Unlocked, levelConfig.Cost);
    }

    private void SetButtonState(Button button, bool active)
    {
        // TODO
    }

    // public Button nextButton;
    // public Button prevButton;

    // public GameObject[] levels;
    // public Image[] levelIndicators; // ������ ����������� Image, ������������ ����� ����

    // private int currentLevelIndex = 0;

    // private void Start()
    // {
    //     ShowCurrentLevel();
    // }

    // public void ShowCurrentLevel()
    // {
    //     for (int i = 0; i < levels.Length; i++)
    //     {
    //         levels[i].SetActive(i == currentLevelIndex);
    //     }

    //     UpdateLevelIndicators();
    //     prevButton.interactable = currentLevelIndex > 0;
    //     nextButton.interactable = currentLevelIndex < levels.Length - 1;
    // }

    // private void UpdateLevelIndicators()
    // {
    //     for (int i = 0; i < levelIndicators.Length; i++)
    //     {
    //         if (i == currentLevelIndex)
    //         {
    //             levelIndicators[i].color = Color.green; // �������� ���� ������� ����� ����
    //         }
    //         else{
    //             levelIndicators[i].color = Color.gray;
    //         }
    //     }
    // }

    // public void GoToNextLevel()
    // {
    //     if (currentLevelIndex < levels.Length - 1)
    //     {
    //         currentLevelIndex++;
    //         ShowCurrentLevel();
    //     }
    // }

    // public void GoToPrevLevel()
    // {
    //     if (currentLevelIndex > 0)
    //     {
    //         currentLevelIndex--;
    //         ShowCurrentLevel();
    //     }
    // }
}
