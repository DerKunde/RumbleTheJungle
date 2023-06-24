using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attackregulation : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Damageable>(out var d))
        {
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
