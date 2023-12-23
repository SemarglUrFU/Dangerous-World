using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance {get; private set;}

    private void Awake() => Instance = this;
    private void OnDestroy() => Instance = null;
}
