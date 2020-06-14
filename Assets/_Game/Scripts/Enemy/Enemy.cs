using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth, IDamagable, IKnockable
{
    [SerializeField]
    private int startingHealth = 100;

    public int Health { get ; set; }
    public bool Alive { get; set; } = true;

    private Rigidbody2D rigidbody;

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Health = startingHealth;
    }

    public virtual void OnDamaged(int amount)
    {
        if (Alive)
        {
            Health -= amount;

            GameObject obj = ObjectPooler.GetPooledObject(SpawnObjectsManager.Instance.GetPrefab(DataKeys.SpawnableKeys.WorldSpaceText));
            obj.GetComponent<WorldSpaceText>().DisplayText("-" + amount, Color.red, transform.position + transform.up, 3, 1);

            if (Health <= 0)
            {
                OnDeath();
            }
        }
    }

    protected virtual void OnDeath()
    {
        Alive = false;
    }

    public void OnKnockback(float knockback, Vector2 direction)
    {
        if (Alive)
        {
            rigidbody.AddForce(direction * knockback, ForceMode2D.Impulse);
        }
    }
}
