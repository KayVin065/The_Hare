using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    // Movement
    public float moveSpeed = 8f;
    public float sprintMultiplier = 1.5f;
    private float currentSpeed;
    public float acceleration = 30f;
    public float deceleration = 120f;
    public float velPower = 0.6f;

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
    public float groundRadius = 0.3f;
    public LayerMask groundLayer;

    // Special Action Objs
    private GameObject nearbyFood;
    private GameObject nearbyDig;
    private GameObject nearbyInvestigate;

    private bool isGrounded;
    private bool isSprinting;

    // Stamina
    private Stamina stamina;
    public float maxStamina = 150f; 
    [SerializeField] private UnityEngine.UI.Slider staminaBar;

    public float sprintDecrease = 5f;
    public float specialDecrease = 10f;
    public float staminaIncrease = 15f;
    private float staminaMultiplier = 1.5f;

    // Lives
    private PlayerLives lives;
    [SerializeField] private LivesUI livesUI;
    private int maxLives = 3;
    private bool isInvincible = false;
    private float invincibleTime = 1f;
    public event System.Action<int> OnLivesChanged;

    // Player Components
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 moveInput;

    // Audio
    public AudioClip jumpSound;
    public AudioClip damageSound;

    [SerializeField] private GameObject loseText;

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
        playerInput.Player.Dig.performed += ctx => TryDig();
        playerInput.Player.Investigate.performed += ctx => TryInvestigate();
        
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }

    void Start()
    {
        stamina = new Stamina(maxStamina);
        lives = new PlayerLives(maxLives);

        isSprinting = false;

        livesUI.Connect(this);
        livesUI.UpdateLives(lives.CurrentLives);

        loseText.SetActive(false);
    }

    void Update()
    {
        moveInput = playerInput.Player.Move.ReadValue<Vector2>();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

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
        float targetSpeed = moveInput.x * currentSpeed;
        float speedDiff = targetSpeed - rb.linearVelocityX;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);
        rb.linearVelocity = new Vector2(rb.linearVelocityX + movement * Time.fixedDeltaTime, rb.linearVelocityY);

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D obj) 
    {
        if (obj.GetComponent<Edible>() != null)
            nearbyFood = obj.gameObject;
        
        if (obj.GetComponent<Diggable>() != null)
            nearbyDig = obj.gameObject;

        if (obj.GetComponent<Investigatable>() != null)
            nearbyInvestigate = obj.gameObject;

        if (obj.CompareTag("Enemy"))
            TakeHit();
    }

    private void OnTriggerExit2D(Collider2D obj) 
    {
        if (obj.gameObject == nearbyFood)
            nearbyFood = null;
        
        if (obj.gameObject == nearbyDig)
            nearbyDig = null;

        if (obj.gameObject == nearbyInvestigate)
            nearbyInvestigate = null;
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
        AudioManager.instance.PlaySFX(jumpSound);

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
        if (nearbyFood == null) {
            Debug.Log("You can't eat that!");
            return;
        }    

        Eat();
    }

    private void Eat()
    {
        Edible edibleObj = nearbyFood.GetComponent<Edible>();

        if (edibleObj != null) {
            edibleObj.Eat();
            stamina.IncreaseStamina(staminaIncrease);

            Debug.Log("You ate something");

            nearbyFood = null;
        }
    }

    // Dig
    private void TryDig()
    {
        if (nearbyDig == null) {
            Debug.Log("You can't dig here!");
            return;
        }

        Dig();
    }

    private void Dig() 
    {
        Diggable diggableObj = nearbyDig.GetComponent<Diggable>();

        if (diggableObj != null) {
            diggableObj.Dig();
            stamina.DecreaseStamina(specialDecrease * 0.5f);

            if(diggableObj.currentDigs >= diggableObj.digsRequired)
                nearbyDig = null;
        }
    }

    // Investigate
    private void TryInvestigate()
    {
        if (nearbyInvestigate == null) {
            Debug.Log("You can't investigate that!");
            return;
        }

        Investigate();

    }

    // Investigating increases the player's stamina and adds a life
    // A cutscene will play when the object is investigated
    private void Investigate()
    {
        Investigatable investigateObj = nearbyInvestigate.GetComponent<Investigatable>();

        if (investigateObj != null) {
            investigateObj.Investigate();
            stamina.IncreaseStamina(staminaIncrease * staminaMultiplier);

            lives.GainLife();
            OnLivesChanged?.Invoke(lives.CurrentLives);
        }
    }

    // Damage
    private void TakeHit()
    {
        if (isInvincible) return;

        lives.LoseLife();
        OnLivesChanged?.Invoke(lives.CurrentLives);
        Debug.Log("YOWCHHHHHH!!!!!");
        AudioManager.instance.PlaySFX(damageSound);

        if(lives.IsDead())
            Die();
        else   
            StartCoroutine(Invincible());
    }

    private IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("You die!");
        GameOver();
    }

    // ********** VISUALS **********

    /*
        - update animations so they change based on each action
    */

    void FlipSprite()
    {
        if (moveInput.x > 0)
            sr.flipX = false;
        else if (moveInput.x < 0)
            sr.flipX = true;
    }

    void GameOver() 
    {
        Debug.Log("Game Over!");
        loseText.SetActive(true);

        this.enabled = false; 
    }
}
