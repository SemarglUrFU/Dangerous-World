using System;
using UnityEngine;

public class CoinsCollector : MonoBehaviour
{   
    public Action OnPick;
    public Action OnRemove;

    public void Pick() => OnPick?.Invoke();
    public void Remove() => OnRemove?.Invoke();
}