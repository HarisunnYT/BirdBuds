using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsMovement : BaseMovement
{
    public override void Update(float time)
    {
        rigidbody.gravityScale = player.HoldingJump ? movementData.GetValue(DataKeys.VariableKeys.GlideGravityScale) : movementData.GetValue(DataKeys.VariableKeys.GravityScale);
    }
}
