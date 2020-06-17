using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataKeys
{
    public class VariableKeys
    {
        //movement keys -------------------------------------------------------------------------------------- \\\\\\\\\\\\\\\\\\\\
        public const string MaxHorizontalSpeed = "max_horizontal_speed";
        public const string MaxVerticalSpeed = "max_vertical_speed";
        public const string HorizontalAcceleration = "horizontal_acceleration";
        public const string VerticalAcceleration = "vertical_acceleration";
        public const string GravityScale = "gravity_scale";
        public const string GroundedLinearDrag = "grounded_linear_drag";
        public const string FlyingLinearDrag = "flying_linear_drag";
        public const string JumpForce = "jump_force";
        public const string JumpRequiresGrounded = "jump_requires_grounded";
        public const string TimeBetweenJump = "time_between_jump";
        public const string ResetVerticalVelocityOnJump = "reset_vertical_velocity_on_jump";
        public const string RotateTowardsDirection = "rotate_towards_direction";
        public const string RotationSpeed = "rotation_speed";
        public const string ConstantMovement = "constant_movement";

        //wing keys -------------------------------------------------------------------------------------- \\\\\\\\\\\\\\\\\\\\
        public const string GlideGravityScale = "glide_gravity_scale";

        //combat keys -------------------------------------------------------------------------------------- \\\\\\\\\\\\\\\\\\\\\\
        public const string AttackSpeedDamper = "attack_speed_damper";

        //technical keys -------------------------------------------------------------------------------------- \\\\\\\\\\\\\\\\\\\
        public const string AttackingButtonResetDelay = "attacking_button_reset_delay";
        public const string FlipScaleDamper = "flip_scale_damper";
    }

    public class SpawnableKeys
    {
        //spawnable object keys -------------------------------------------------------------------------------------- \\\\\\\\\\\\\\\\\\\
        public const string WorldSpaceText = "world_space_text";
    }
}
