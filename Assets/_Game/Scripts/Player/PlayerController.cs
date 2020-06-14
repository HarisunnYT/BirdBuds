using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    #region PLAYER_EXTENSIONS

    public enum MovementType
    {
        Normal,
        Copter,
        Cloth,

        WingsMini,
        WingsMid,
        WingsBig,

        BirdRider,
        MagicRider,
        CloudRider,

        Balloon,
        BackPack,
        Glider
    }

    [System.Serializable]
    struct MovementTypeData
    {
        public MovementType MovementType;
        public CharacterData MovementData;
    }

    #endregion

    #region EXPOSED_VARIABLES

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
    private GameObject scaleFlipper;

    [SerializeField]
    private GameObject transformParticle;

    #endregion

    #region COMPONENTS

    public CharacterData CurrentMovementData { get; private set; }
    public MovementType CurrentMovementType { get; private set; } = MovementType.Normal;

    private BaseMovement baseMovement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    #endregion

    #region RUNTIME_VARIABLES

    public Vector2 InputAxis { get; private set; }

    public bool Grounded { get; private set; } = false;
    public bool HoldingJump { get; private set; } = false;
    public int Direction { get; private set; } = 1;
    public bool HorizontalMovementEnabled { get; private set; } = true;
    public bool VerticalMovementEnabled { get; private set; } = true;


    private Vector3 originalScale;
    private LayerMask invertedPlayerMask;

    private float previousScaleSwappedTimer = 0;
    private float attackButtonTimer = 0;
    private float timeBetweenJumpTimer = 0;

    private bool attacking = false;

    private Coroutine horizontalMovementCoroutine;
    private Coroutine verticalMovementCoroutine;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
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
            if (transformType > MovementType.BackPack)
                transformType = MovementType.Copter;
            else if (transformType < MovementType.Copter)
                transformType = MovementType.BackPack;

            SetMovementType(transformType);
        }

        //flip scale
        if (InputAxis.x != 0 && Time.time > previousScaleSwappedTimer)
        {
            Direction = roundedXAxis;
            spriteRenderer.flipX = Direction == 1 ? false : true;
            scaleFlipper.transform.localScale = new Vector3(Direction, 1, 1);

            previousScaleSwappedTimer = Time.time + technicalData.GetValue(DataKeys.VariableKeys.FlipScaleDamper);
        }

        if (Input.GetButtonDown("Jump") && Time.time > timeBetweenJumpTimer)
        {
            baseMovement.Jump();
            timeBetweenJumpTimer = Time.time + CurrentMovementData.GetValue(DataKeys.VariableKeys.TimeBetweenJump);
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
            attackButtonTimer = Time.time + TechnicalData.GetValue(DataKeys.VariableKeys.AttackingButtonResetDelay);
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

        if (HorizontalMovementEnabled)
            baseMovement.MoveHorizontal(Time.deltaTime);

        if (VerticalMovementEnabled)
            baseMovement.MoveVertical(Time.deltaTime);
    }

    #region MOVEMENT

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

        Rigidbody.gravityScale = CurrentMovementData.GetValue(DataKeys.VariableKeys.GravityScale);
        Rigidbody.drag = CurrentMovementData.GetValue(DataKeys.VariableKeys.LinearDrag);

        if (baseMovement != null)
            baseMovement.Deconfigure();

        if (CurrentMovementType >= MovementType.WingsMini && CurrentMovementType <= MovementType.WingsBig)
            baseMovement = new WingsMovement().Configure(this, Rigidbody, animator);
        else
            baseMovement = new BaseMovement().Configure(this, Rigidbody, animator);

        if (displayPuffParticle)
        {
            GameObject particle = ObjectPooler.GetPooledObject(transformParticle);
            particle.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, particle.transform.position.z);
        }
    }

    public float GetMaxHorizontalSpeed()
    {
        return attacking ? CurrentMovementData.GetValue(DataKeys.VariableKeys.MaxHorizontalSpeed) / CurrentMovementData.GetValue(DataKeys.VariableKeys.AttackSpeedDamper) :
                           CurrentMovementData.GetValue(DataKeys.VariableKeys.MaxHorizontalSpeed);
    }

    public float GetMaxVerticalSpeed()
    {
        return CurrentMovementData.GetValue(DataKeys.VariableKeys.MaxVerticalSpeed);
    }

    public void Knockback(Vector3 direction, float force)
    {
        DisableHorizontalMovement(0.5f);

        Rigidbody.velocity = new Vector2(0, Rigidbody.velocity.y);
        Rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }

    #endregion

    #region COMBAT

    public override void OnDamaged(int amount)
    {
        base.OnDamaged(amount);

        SetMovementType(MovementType.Normal);
    }

    #endregion

    #region DISABLE_INPUT

    public void DisableHorizontalMovement(float duration)
    {
        if (horizontalMovementCoroutine != null)
            StopCoroutine(horizontalMovementCoroutine);

        HorizontalMovementEnabled = false;
        horizontalMovementCoroutine = StartCoroutine(DisableHorizontalMovementCoroutine(duration));
    }

    private IEnumerator DisableHorizontalMovementCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        HorizontalMovementEnabled = true;
        horizontalMovementCoroutine = null;
    }

    public void DisableVerticalMovement(float duration)
    {
        if (verticalMovementCoroutine != null)
            StopCoroutine(verticalMovementCoroutine);

        VerticalMovementEnabled = false;
        verticalMovementCoroutine = StartCoroutine(DisableVerticalMovementCoroutine(duration));
    }

    private IEnumerator DisableVerticalMovementCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        VerticalMovementEnabled = true;
        verticalMovementCoroutine = null;
    }

    #endregion
}
