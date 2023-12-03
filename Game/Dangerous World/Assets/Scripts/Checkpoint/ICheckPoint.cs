using UnityEngine;

public interface ICheckPoint
{
    public Vector2 Position {get;}
    public void UpdateStatus(bool isActive);
}
