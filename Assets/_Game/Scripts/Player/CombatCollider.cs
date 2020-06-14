using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCollider : MonoBehaviour
{
    //TODO make weapon data
    public int Damage;
    public float Knockback = 5;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.OnDamaged(Damage);
            playerController.Knockback((transform.position - other.transform.position).normalized, playerController.CharacterStats.Weight);
        }

        IKnockable knockable = other.GetComponent<IKnockable>();
        if (knockable != null)
            knockable.OnKnockback(Knockback, (other.transform.position - transform.position).normalized + Vector3.up);
    }
}
