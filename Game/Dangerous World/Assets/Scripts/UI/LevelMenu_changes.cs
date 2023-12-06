using UnityEngine;
using UnityEngine.UI;

public class LevelMenu_changes : MonoBehaviour
{
    public Button nextButton;
    public Button prevButton;

    public GameObject[] levels;
    public Image[] levelIndicators; // массив компонентов Image, отображающих части бара

    private int currentLevelIndex = 0;

    private void Start()
    {
        ShowCurrentLevel();
    }

    public void ShowCurrentLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(i == currentLevelIndex);
        }

        UpdateLevelIndicators();
        prevButton.interactable = currentLevelIndex > 0;
        nextButton.interactable = currentLevelIndex < levels.Length - 1;
    }

    private void UpdateLevelIndicators()
    {
        for (int i = 0; i < levelIndicators.Length; i++)
        {
            if (i == currentLevelIndex)
            {
                levelIndicators[i].color = Color.black; // изменяем цвет текущей части бара
            }
            else{
                levelIndicators[i].color = Color.gray;
            }
        }
    }

    public void GoToNextLevel()
    {
        if (currentLevelIndex < levels.Length - 1)
        {
            currentLevelIndex++;
            ShowCurrentLevel();
        }
    }

    public void GoToPrevLevel()
    {
        if (currentLevelIndex > 0)
        {
            currentLevelIndex--;
            ShowCurrentLevel();
        }
    }
}
