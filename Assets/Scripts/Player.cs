using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Movement
    public float moveSpeed = 12f;
    public float sprintMultiplier = 1.5f;
    private float currentSpeed;

    // Jump
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f;

    // Jump Assist
    private float coyoteTime = 0.1f;
    private float jumpBufferTime = 0.1f;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    // Ground Check
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    // Food Check
    public Transform eatCheck;
    public float eatRadius = 0.5f;
    public LayerMask foodLayer;
    private GameObject nearbyFood;

    // Stamina
    private Stamina stamina;
    public float maxStamina = 150f; 
    [SerializeField] private UnityEngine.UI.Slider staminaBar;

    public float sprintDecrease = 5f;
    public float specialDecrease = 10f;
    public float staminaIncrease = 15f;
    private float staminaMultiplier = 1.5f;

    // Player Components
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 moveInput;

    private bool isGrounded;
    private bool isSprinting;

    // Abilities
    private bool isEdible = false;
    //private bool isDiggable = true;

    private void Awake()
    {
        playerInput = new PlayerInput();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        currentSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();

        playerInput.Player.Jump.performed += OnJump;

        playerInput.Player.Sprint.started += OnSprintStart;
        playerInput.Player.Sprint.canceled += OnSprintStop;

        playerInput.Player.Eat.performed += ctx => TryEat();
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }

    void Start()
    {
        stamina = new Stamina(maxStamina);
        isSprinting = false;
    }

    void Update()
    {
        moveInput = playerInput.Player.Move.ReadValue<Vector2>();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        Collider2D food = Physics2D.OverlapCircle(eatCheck.position, eatRadius, foodLayer);
        isEdible = (food != null);

        // Check for nearby food objects
        if(isEdible) 
            nearbyFood = food.gameObject;
        else
            nearbyFood = null;

        // Jump timer and counter updates
        if(isGrounded) 
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0) {
            Jump();
            jumpBufferCounter = 0;
        }

        // Stamina calculations
        if(moveInput.x != 0) 
        {
            // Decrease stamina more while sprinting
            if(isSprinting)
                stamina.DecreaseStamina(sprintDecrease * Time.deltaTime);
        }

        // Stamina UI 
        if(staminaBar != null)
            staminaBar.value = stamina.CurrentStamina / stamina.MaxStamina;

        if (stamina.CurrentStamina <= 0)
            GameOver();

        FlipSprite();
    }

    private void FixedUpdate() 
    {
        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocityY);

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // ********** ACTIONS **********
    // Jump
    void OnJump(InputAction.CallbackContext ctx) 
    {
        jumpBufferCounter = jumpBufferTime;
    }

    private void Jump()
    {
        coyoteTimeCounter = 0;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);

        stamina.DecreaseStamina(specialDecrease);
    }

    // Sprint
    void OnSprintStart(InputAction.CallbackContext ctx) 
    {
        isSprinting = true;
        currentSpeed = moveSpeed * sprintMultiplier;
    }

    void OnSprintStop(InputAction.CallbackContext ctx) 
    {
        isSprinting = false;
        currentSpeed = moveSpeed;
    }

    // Eat
    private void TryEat()
    {
        if (!isEdible || nearbyFood == null) {
            Debug.Log("You can't eat that!");
            return;
        }    

        Eat();
    }

    private void Eat()
    {
        Debug.Log("You ate something");
        Destroy(nearbyFood); // eat the plant
        nearbyFood = null;
        isEdible = false;

        stamina.IncreaseStamina(staminaIncrease);
    }

    // Dig

    // ********** VISUALS **********

    /*
        - update animations so they change based on each action
        - update sounds to play with certain actions (do i just put this inside of the callback functions?)
    */

    void FlipSprite()
    {
        if (moveInput.x > 0)
            sr.flipX = false;
        else if (moveInput.x < 0)
            sr.flipX = true;
    }

    // ********** GET RID OF THIS LATER ********** 
    void OnDrawGizmosSelected()
    {
        if (eatCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(eatCheck.position, eatRadius);
        }
    }

    void GameOver() 
    {
        Debug.Log("Game Over!");
        this.enabled = false; 
    }
}
