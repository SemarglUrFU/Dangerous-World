using System;

public class CoinsCounter
{
    public Action OnAdd;
    public Action OnRemove;

    public int Collected { get; private set; }
    public int Total { get; private set; }

    public void Initialize(int total, int collected = 0)
    {
        Total = total;
        Collected = (collected < 0) ? total : collected;
    }

    public void Add()
    {
        Collected += 1;
        OnAdd?.Invoke();
    }

    public void Remove()
    {
        Collected -= 1;
        OnRemove?.Invoke();
    }
}