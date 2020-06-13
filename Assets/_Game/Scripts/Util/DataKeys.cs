using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataKeys
{
    //movement keys -------------------------------------------------------------------------------------- \\\\\\\\\\\\\\\\\\\\
    public const string MaxHorizontalSpeed = "max_horizontal_speed";
    public const string MaxVerticalSpeed = "max_vertical_speed";
    public const string HorizontalAcceleration = "horizontal_acceleration";
    public const string VerticalAcceleration = "vertical_acceleration";
    public const string GravityScale = "gravity_scale";
    public const string LinearDrag = "linear_drag";
    public const string JumpForce = "jump_force";
    public const string JumpRequiresGrounded = "jump_requires_grounded";

    //combat keys -------------------------------------------------------------------------------------- \\\\\\\\\\\\\\\\\\\\\\
    public const string AttackSpeedDamper = "attack_Speed_Damper";

    //technical keys -------------------------------------------------------------------------------------- \\\\\\\\\\\\\\\\\\\
    public const string AttackingButtonResetDelay = "attacking_button_reset_delay";
    public const string FlipScaleDamper = "flip_scale_damper";
}
