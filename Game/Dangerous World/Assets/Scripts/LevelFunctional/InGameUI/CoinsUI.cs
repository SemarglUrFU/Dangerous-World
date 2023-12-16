using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] TMP_Text _collected;
    [SerializeField] TMP_Text _count;

    public void Initialize(int count, int collected)
    {
        SetCount(count);
        SetCollected(collected);
    }

    public void SetCollected(int collected) => _collected.text = collected.ToString();
    public void SetCount(int count) => _count.text = count.ToString();
}
