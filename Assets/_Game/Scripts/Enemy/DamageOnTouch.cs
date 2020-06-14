using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour, IDamages
{
    [SerializeField]
    private int damage;

    public int Damage { get { return damage; } set { damage = value; } }

    
}
