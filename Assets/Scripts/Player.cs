using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float sprintSpeed = 5f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 moveInput;

    private string GROUND_TAG = "Ground";
    private string FOOD_TAG = "Plant";
    //private string DIG_TAG = "Dig";

    private bool isGrounded = true;
    private bool isEdible = false;
    //private bool isDiggable = true;

    private void Awake()
    {
        playerInput = new PlayerInput();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();

        playerInput.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        playerInput.Player.Jump.performed += ctx => TryJump();

        playerInput.Player.Sprint.started += ctx => moveSpeed += sprintSpeed;
        playerInput.Player.Sprint.canceled += ctx => moveSpeed -= sprintSpeed;

        playerInput.Player.Eat.performed += ctx => TryEat();
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(move);

        if(rb.linearVelocityY < 0)
            rb.linearVelocityY += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }

    private void TryJump()
    {
        if (!isGrounded) return;

        Jump();
    }
    private void Jump()
    {
        Debug.Log("Jump pressed");
        isGrounded = false;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
    }

    private void TryEat()
    {
        if (!isEdible) {
            Debug.Log("You can't eat that!");
            return;
        }    
        Eat();
    }

    private void Eat()
    {
        Debug.Log("You ate something");
        isEdible = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(GROUND_TAG)) {
            isGrounded = true;
            Debug.Log("Ground touched");
        }

        if(collision.gameObject.CompareTag(FOOD_TAG))
        {
            isEdible = true;
        }

    }

}
