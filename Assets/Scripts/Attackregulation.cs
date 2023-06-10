using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attackregulation : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        Damageable d = other.GetComponent<Damageable>();
        d.Hit(damage);
    }
    
    public void OnDirectionChanged(bool facingRight)
    {
        var position = transform.position;
        position.x *= -1;
        transform.position = position;
    }
}
