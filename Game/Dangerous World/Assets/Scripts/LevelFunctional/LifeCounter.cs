using System;
public class LifeCounter
{
    public Action OnLifesOver;
    public Action OnLifesAdd;
    public Action OnLifesSpend;
    public Action<int> OnSet;

    public int Count { get; private set; }
    public int Left { get; private set; }

    public void Initialize(int count, int left = -1)
    {
        Count = count;
        Left = (left < 0 || left > count) ? count : left;
    }

    public bool Spend()
    {
        Left -= 1;
        if (Left < 0) { Left = 0; return false; }
        else { OnLifesSpend?.Invoke(); }
        if (Left == 0) { OnLifesOver?.Invoke(); return false; }
        return true;
    }

    public bool Add()
    {
        if (++Left > Count) { Left = Count; return false; }
        OnLifesAdd?.Invoke();
        return true;
    }

    public void Set(int count = -1)
    {
        if (count > Count || count < 0) { Left = Count; }
        else { Left = count; }
        OnSet?.Invoke(Left);
    }
}