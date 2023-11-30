using UnityEngine;
using UnityEngine.Events;

public class MeleeDamage : MonoBehaviour
{
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] UnityEvent _OnDamageApply;

    private void OnCollisionEnter2D(Collision2D collision){ 
        if (!collision.collider.TryGetComponent<IDamageble>(out var damageble) ||
            !damageble.ApplyDamage())
            return;
        
        _OnDamageApply.Invoke();
    }
}

