using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCollider : MonoBehaviour
{
    //TODO make weapon data
    public int Damage;
    public float Knockback = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null)
            damagable.OnDamaged(Damage);

        IKnockable knockable = other.GetComponent<IKnockable>();
        if (knockable != null)
            knockable.OnKnockback(Knockback, (other.transform.position - transform.position) + Vector3.up);
    }
}
