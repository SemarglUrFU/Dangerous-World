using System;
using UnityEngine;

public class CoinsCounter
{
    public Action OnAdd;
    public Action OnRemove;

    public int Сollected { get; private set; }
    public int Total { get; private set; }

    public void Initialize(int total, int collected = 0)
    {
        Total = total;
        Сollected = (collected < 0) ? total : collected;
    }

    public void Add()
    {
        Сollected += 1;
        OnAdd?.Invoke();
    }

    public void Remove()
    {
        Сollected -= 1;
        OnRemove?.Invoke();
    }
}