using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum MovementType
    {
        Normal,
        Copter,
        Cloth,
        WingsMini,
        WingsMid
    }

    [System.Serializable]
    struct MovementTypeData
    {
        public MovementType MovementType;
        public CharacterData MovementData;
    }

    [Header("DEBUG TRANSFORM TYPE")]
    [SerializeField]
    private MovementType transformType;
    [Space()]

    [SerializeField]
    private MovementTypeData[] movementTypeDataItems;

    [SerializeField]
    private CharacterData technicalData;
    public CharacterData TechnicalData { get { return technicalData; } }

    public CharacterData CurrentMovementData { get; private set; }
    public MovementType CurrentMovementType { get; private set; } = MovementType.Normal;

    public Vector2 InputAxis { get; private set; }

    public bool Grounded { get; private set; } = false;

    private BaseMovement baseMovement;

    private Animator animator;
    private Rigidbody2D rigidbody;

    private Vector3 originalScale;

    private int previousScale = 1;
    private float previousScaleSwappedTimer = 0;

    private bool attacking = false;
    private float attackButtonTimer = 0;

    private float timeBetweenJumpTimer = 0;

    private LayerMask invertedPlayerMask;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;

        SetMovementType(MovementType.Normal);

        //8 == player layer
        invertedPlayerMask = ~(1 << 8);
    }

    private void Update()
    {
        InputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int roundedXAxis = InputAxis.x > 0 ? 1 : -1;

        animator.SetBool("Running", InputAxis.x != 0);

        //flip scale
        if (InputAxis.x != 0 && Time.time > previousScaleSwappedTimer)
        {
            transform.localScale = new Vector3(originalScale.x * roundedXAxis, transform.localScale.y, transform.localScale.z);
            previousScale = (int)transform.localScale.x;

            previousScaleSwappedTimer = Time.time + technicalData.GetValue(DataKeys.FlipScaleDamper);
        }

        if (Input.GetButtonDown("Jump") && Time.time > timeBetweenJumpTimer)
        {
            baseMovement.Jump();
            timeBetweenJumpTimer = Time.time + CurrentMovementData.GetValue(DataKeys.TimeBetweenJump);
        }

        if (Input.GetButtonDown("Transform"))
        {
            SetMovementType(CurrentMovementType == MovementType.Normal ? transformType : MovementType.Normal);
        }

        //attacking
        if (Input.GetButtonDown("Attack"))
        {
            baseMovement.Attack();
            attacking = true;
            attackButtonTimer = Time.time + TechnicalData.GetValue(DataKeys.AttackingButtonResetDelay);
        }

        if (Time.time > attackButtonTimer)
        {
            attacking = false;
        }

        Grounded = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.down, 0.5f, invertedPlayerMask);

        animator.SetBool("Attacking", attacking);
        animator.SetBool("Grounded", Grounded);
    }

    private void FixedUpdate()
    {
        baseMovement.Update(Time.time);
        baseMovement.Move();
    }

    public float GetMaxHorizontalSpeed()
    {
        return attacking ? CurrentMovementData.GetValue(DataKeys.MaxHorizontalSpeed) / CurrentMovementData.GetValue(DataKeys.AttackSpeedDamper) : CurrentMovementData.GetValue(DataKeys.MaxHorizontalSpeed);
    }

    public float GetMaxVerticalSpeed()
    {
        return CurrentMovementData.GetValue(DataKeys.MaxVerticalSpeed);
    }

    public void SetMovementType(MovementType movementType)
    {
        for (int i = 0; i < System.Enum.GetNames(typeof(MovementType)).Length; i++)
        {
            animator.SetLayerWeight(animator.GetLayerIndex(((MovementType)i).ToString()), 0);
        }

        animator.SetLayerWeight(animator.GetLayerIndex((movementType).ToString()), 1);
        CurrentMovementType = movementType;

        foreach (var movementTypeDataItem in movementTypeDataItems)
        {
            if (movementTypeDataItem.MovementType == movementType)
            {
                CurrentMovementData = movementTypeDataItem.MovementData;
            }
        }

        rigidbody.gravityScale = CurrentMovementData.GetValue(DataKeys.GravityScale);
        rigidbody.drag = CurrentMovementData.GetValue(DataKeys.LinearDrag);

        baseMovement = new BaseMovement().Configure(this, rigidbody, animator);
    }
}
