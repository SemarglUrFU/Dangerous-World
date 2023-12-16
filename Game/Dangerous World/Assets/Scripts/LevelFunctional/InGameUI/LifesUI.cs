using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class LifesUI : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _gridLayout;
    [SerializeField] private LifeImage _prefab;
    [SerializeField] private int _lifeCountDefault;
    
    public int Left => _lifesLeft;
    public int Count => _lifeCount;

    private int _lifesLeft = 0;
    private int _lifeCount = 0;
    private readonly List<LifeImage> _lifes = new();
    
    public void Initialize(int lifesCount, int lifesLeft = -1)
    {
        if (lifesCount != _lifeCount)
        {
            foreach (var life in GetComponentsInChildren<LifeImage>()) { DestroyImmediate(life.gameObject); }
            _lifeCount = lifesCount;
            _gridLayout.constraintCount = lifesCount;
            _lifes.Clear();
            _lifes.Capacity = lifesCount;
            for (int i = 0; i < lifesCount; i++) { _lifes.Add(Instantiate(_prefab, transform)); }
        }
        _lifesLeft = (lifesLeft < 0 || lifesLeft > _lifeCount) ? _lifeCount : lifesLeft;
        for (int i = 0; i < _lifesLeft; i++) { _lifes[i].Initialize(true); }
        for (int i = _lifesLeft; i < lifesCount; i++) { _lifes[i].Initialize(false); }
    }

    public void Spend()
    {
        if (--_lifesLeft < 0) { _lifesLeft = 0; }
        else { _lifes[_lifesLeft].Disable();}
    }

    public void Add()
    {
        if (++_lifesLeft > _lifeCount) { _lifesLeft = _lifeCount; }
        else {_lifes[_lifesLeft-1].Enable();}
    }

    private void OnValidate()
    {
        _gridLayout ??= GetComponent<GridLayoutGroup>();
    }
    [ContextMenu("ReInitialize")] private void __ReInitialize() {Initialize(0); Initialize(_lifeCountDefault);}
    [ContextMenu("Spend")] private void __Spend() => Spend();
    [ContextMenu("Add")] private void __Add() => Add();
}
