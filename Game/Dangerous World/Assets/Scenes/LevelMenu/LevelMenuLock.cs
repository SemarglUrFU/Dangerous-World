using TMPro;
using UnityEngine;

class LevelMenuLock : MonoBehaviour
{
    [SerializeField] LevelMenuStars _levelMenuStars;
    [SerializeField] TextMeshPro _costText;
    
    public void UpdateSelf(bool unlocked = false, int cost = 0)
    {
        //? (unlocked == false, cost == 0) -- Уровень разблокирован изначально
        // TODO
    }
}