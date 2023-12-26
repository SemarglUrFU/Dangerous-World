using UnityEngine;

[RequireComponent(typeof(Collider2D))] 
public class OutOfBounds : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent<IDamageble>(out var damageble))
        {
            damageble.ApplyDamage();
        }
    }
}
