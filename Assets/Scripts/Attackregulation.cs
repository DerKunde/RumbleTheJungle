using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attackregulation : MonoBehaviour
{
    public int damage = 1;
    public UnityEvent onDamageableHit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Damageable>(out var d))
        {
            onDamageableHit?.Invoke();
            d.Hit(damage);
        }

    }
    
    public void OnDirectionChanged(bool facingRight)
    {
        var position = transform.localPosition;
        position.x *= -1;
        transform.localPosition = position;
    }
}
