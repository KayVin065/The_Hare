using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 10f;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 moveInput;

    private string GROUND_TAG = "Ground";
    private bool isGrounded = true;

    private void Awake()
    {
        playerInput = new PlayerInput();
        
        playerInput.Player.Jump.performed += ctx => TryJump();
        // space here for other events subscribed to 
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveInput = playerInput.Player.Move.ReadValue<Vector2>();
        transform.Translate(moveInput * moveSpeed * Time.deltaTime);
    }

    private void TryJump()
    {
        if (!isGrounded) return;

        Jump();
    }
    private void Jump()
    {
        Debug.Log("Jump pressed");
    }


}
