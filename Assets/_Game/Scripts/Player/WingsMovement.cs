using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsMovement : BaseMovement
{
    public override void Move(float deltaTime)
    {
        base.Move(deltaTime);

        rigidbody.gravityScale = player.HoldingJump ? movementData.GetValue(DataKeys.GlideGravityScale) : movementData.GetValue(DataKeys.GravityScale);
    }
}
