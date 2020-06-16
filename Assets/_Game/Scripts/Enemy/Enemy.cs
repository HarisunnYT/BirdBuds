using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas;

public class Enemy : Character
{
    #region EXPOSED_VARIABLES

    [SerializeField]
    private CharacterData movementData;

    [SerializeField]
    private CharacterData technicalData;

    #endregion

    #region RUNTIME_VARIABLES

    private float previousScaleSwappedTimer = 0;

    #endregion

    private void Start()
    {
        Rigidbody.isKinematic = !isServer;
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time > previousScaleSwappedTimer && ((Rigidbody.velocity.x > 0 && Direction == 1) || (Rigidbody.velocity.x < 0 && Direction == -1)))
        {
            Direction = Rigidbody.velocity.x > 0 ? -1 : 1;

            SetDirection(Direction);

            previousScaleSwappedTimer = Time.time + technicalData.GetValue(DataKeys.VariableKeys.FlipScaleDamper);
        }
    }

    #region MOVEMENT

    public void MoveTowards(GameObject obj, float distanceToStop)
    {
        var direction = Vector3.zero;
        if (Vector3.Distance(transform.position, obj.transform.position) > distanceToStop)
        {
            direction = obj.transform.position - transform.position;
            Rigidbody?.AddRelativeForce(direction.normalized * movementData.GetValue("horizontal_acceleration"), ForceMode2D.Force);
        }
    }

    #endregion
}
