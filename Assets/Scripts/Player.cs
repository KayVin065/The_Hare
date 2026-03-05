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
    public float maxStamina = 100f; 
    public float currentStamina;

    public float moveDecreaseRate = 2f; 
    public float sprintDecreaseRate = 7f;
    public float specialDecreaseRate = 10f;
    public float staminaIncreaseRate = 15f;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private UnityEngine.UI.Slider staminaBar;

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
        currentStamina = maxStamina;
        isSprinting = false;
    }

    void Update()
    {
        moveInput = playerInput.Player.Move.ReadValue<Vector2>();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        Collider2D food = Physics2D.OverlapCircle(eatCheck.position, eatRadius, foodLayer);
        isEdible = (food != null);

        if(isEdible) 
            nearbyFood = food.gameObject;
        else
            nearbyFood = null;

        // Jump timer updating
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
            currentStamina -= moveDecreaseRate * Time.deltaTime;

            if(isSprinting)
                currentStamina -= sprintDecreaseRate * Time.deltaTime;

            if(currentStamina < 0) 
                currentStamina = 0
        }

        if(staminaBar != null)
            staminaBar.value = currentStamina / maxStamina;

        if (currentStamina <= 0)
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

    void OnJump(InputAction.CallbackContext ctx) 
    {
        jumpBufferCounter = jumpBufferTime;
    }

    private void Jump()
    {
        coyoteTimeCounter = 0;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);

        currentStamina -= specialDecreaseRate;
        if (currentStamina < 0)
            currentStamina = 0;
    }

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

    void FlipSprite()
    {
        if (moveInput.x > 0)
            sr.flipX = false;
        else if (moveInput.x < 0)
            sr.flipX = true;
    }

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

        currentStamina += staminaIncreaseRate;
        if (currentStamina > maxStamina)
            currentStamina = maxStamina;
    }

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
        Debug.Log("You ran out of stamina!");
        this.enabled = false; 
    }
}
