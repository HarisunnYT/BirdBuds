using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    int Health { get; set; }
    bool Invincible { get; set; }
    bool Alive { get; set; }
}

public interface IDamagable
{
    int Health { get; set; }

    void OnDamaged(int amount);
}

public interface IDamages
{
    int Damage { get; set; }
}

public interface IKnockable
{
    void OnKnockback(float knockback, Vector2 direction);
}