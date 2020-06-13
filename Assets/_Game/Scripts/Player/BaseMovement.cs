using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement
{
    protected PlayerController player;
    protected Rigidbody2D rigidbody;
    protected Animator animator;
    protected CharacterData movementData;

    public virtual BaseMovement Configure(PlayerController player, Rigidbody2D rigidbody, Animator animator)
    {
        this.player = player;
        this.rigidbody = rigidbody;
        this.animator = animator;
        this.movementData = player.CurrentMovementData;

        return this;
    }

    public virtual void Update(float time) { }

    public virtual void Jump() 
    {
        if (movementData.GetValue(DataKeys.JumpRequiresGrounded) == 0 || player.Grounded)
        {
            animator.SetTrigger("Jump");
            rigidbody.AddForce(new Vector2(0, movementData.GetValue(DataKeys.JumpForce)));
        }
    }

    public virtual void Move() 
    {
        //move horizontal
        if ((player.InputAxis.x > 0 && rigidbody.velocity.x < player.GetMaxHorizontalSpeed()) ||
            (player.InputAxis.x < 0 && rigidbody.velocity.x > -player.GetMaxHorizontalSpeed()))
        {
            rigidbody.AddForce(new Vector2(player.InputAxis.x * movementData.GetValue(DataKeys.HorizontalAcceleration), 0));
        }

        rigidbody.AddForce(new Vector2(0, player.InputAxis.y * movementData.GetValue(DataKeys.VerticalAcceleration)));
    }

    public virtual void Attack() { }
}
