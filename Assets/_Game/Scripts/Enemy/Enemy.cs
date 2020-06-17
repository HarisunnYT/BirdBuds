using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas;
using NodeCanvas.StateMachines;

public class Enemy : Character
{
    #region EXPOSED_VARIABLES

    [SerializeField]
    private CharacterData movementData;

    [SerializeField]
    private CharacterData technicalData;

    #endregion

    #region COMPONENTS

    private FSMOwner fsmOwner;
    private PolyNavAgent agent;

    #endregion

    #region RUNTIME_VARIABLES

    private float previousScaleSwappedTimer = 0;

    #endregion

    private void Start()
    {
        fsmOwner = GetComponent<FSMOwner>();
        agent = GetComponent<PolyNavAgent>();

        Rigidbody.isKinematic = !isServer;
        fsmOwner.enabled = isServer;
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time > previousScaleSwappedTimer && ((agent.movingDirection.x > 0 && Direction == 1) || (agent.movingDirection.x < 0 && Direction == -1)))
        {
            Direction = agent.movingDirection.x > 0 ? -1 : 1;

            SetDirection(Direction);

            previousScaleSwappedTimer = Time.time + technicalData.GetValue(DataKeys.VariableKeys.FlipScaleDamper);
        }
    }

    #region MOVEMENT

    #endregion
}
