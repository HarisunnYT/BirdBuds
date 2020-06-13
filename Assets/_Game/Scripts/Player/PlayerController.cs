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
        WingsMid,
        WingsBig,

        BirdRider,
        MagicRider
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

    [Space()]
    [SerializeField]
    private GameObject transformParticle;

    public CharacterData CurrentMovementData { get; private set; }
    public MovementType CurrentMovementType { get; private set; } = MovementType.Normal;

    public Vector2 InputAxis { get; private set; }

    public bool Grounded { get; private set; } = false;
    public bool HoldingJump { get; private set; } = false;
    public int Direction { get; private set; } = 1;

    private BaseMovement baseMovement;

    private Animator animator;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    private Vector3 originalScale;

    private float previousScaleSwappedTimer = 0;

    private bool attacking = false;
    private float attackButtonTimer = 0;

    private float timeBetweenJumpTimer = 0;

    private LayerMask invertedPlayerMask;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

        SetMovementType(MovementType.Normal, false);

        //8 == player layer
        invertedPlayerMask = ~(1 << 8);
    }

    private void Update()
    {
        InputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int roundedXAxis = InputAxis.x > 0 ? 1 : -1;

        animator.SetBool("Running", InputAxis.x != 0);

        //TODO REMOVE
        if (Input.GetButtonDown("Tab"))
        {
            transformType += (int)Input.GetAxisRaw("Tab");
            if (transformType > MovementType.MagicRider)
                transformType = MovementType.Copter;
            else if (transformType < MovementType.Copter)
                transformType = MovementType.MagicRider;

            SetMovementType(transformType);
        }

        //flip scale
        if (InputAxis.x != 0 && Time.time > previousScaleSwappedTimer)
        {
            Direction = roundedXAxis;
            spriteRenderer.flipX = Direction == 1 ? false : true;

            previousScaleSwappedTimer = Time.time + technicalData.GetValue(DataKeys.FlipScaleDamper);
        }

        if (Input.GetButtonDown("Jump") && Time.time > timeBetweenJumpTimer)
        {
            baseMovement.Jump();
            timeBetweenJumpTimer = Time.time + CurrentMovementData.GetValue(DataKeys.TimeBetweenJump);
        }

        HoldingJump = Input.GetButton("Jump");

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
        animator.SetBool("HoldingJump", HoldingJump);
    }

    private void FixedUpdate()
    {
        baseMovement.Update(Time.time);
        baseMovement.Move(Time.deltaTime);
    }

    public float GetMaxHorizontalSpeed()
    {
        return attacking ? CurrentMovementData.GetValue(DataKeys.MaxHorizontalSpeed) / CurrentMovementData.GetValue(DataKeys.AttackSpeedDamper) : CurrentMovementData.GetValue(DataKeys.MaxHorizontalSpeed);
    }

    public float GetMaxVerticalSpeed()
    {
        return CurrentMovementData.GetValue(DataKeys.MaxVerticalSpeed);
    }

    public void SetMovementType(MovementType movementType, bool displayPuffParticle = true)
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

        if (baseMovement != null)
            baseMovement.Deconfigure();

        if (CurrentMovementType >= MovementType.WingsMini && CurrentMovementType <= MovementType.WingsBig)
            baseMovement = new WingsMovement().Configure(this, rigidbody, animator);
        else
            baseMovement = new BaseMovement().Configure(this, rigidbody, animator);

        if (displayPuffParticle)
        {
            GameObject particle = ObjectPooler.GetPooledObject(transformParticle);
            particle.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, particle.transform.position.z);
        }
    }
}
