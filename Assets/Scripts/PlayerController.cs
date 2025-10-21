using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 12f;
    public float jumpForce = 12f;
    public float fallMultiplier = 3f;     // Caída más rápida
    public float lowJumpMultiplier = 2f;  // Salto más bajo si sueltas el botón

    private float moveInput;
    private bool isGrounded;
    private int jumpCount;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        jumpCount = 0;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleBetterJump();
        HandleAnimations();
    }

    void HandleMovement()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.AddForce(new Vector2(moveInput * 50f, 0));

        // Limitar velocidad horizontal
        if (Mathf.Abs(rb.velocity.x) > moveSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * moveSpeed, rb.velocity.y);

        // Voltear sprite según dirección
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }
    }

    void HandleBetterJump()
    {
        if (rb.velocity.y < 0)
        {
            // Caída más rápida
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            // Saltos más bajos si sueltas el botón
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void HandleAnimations()
    {
        bool walking = moveInput != 0 && isGrounded;
        bool jumping = !isGrounded;

        anim.SetBool("IsWalking", walking);
        anim.SetBool("IsJumping", jumping);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
            isGrounded = false;
    }
}
