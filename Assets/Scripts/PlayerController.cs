using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 12f;       // Velocidad horizontal
    public float jumpForce = 12f;       // Fuerza del salto
    public float fallMultiplier = 20f;   // Caída más rápida
    public float moveAcceleration = 50f; // Fuerza para movimiento más responsivo

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
        HandleBetterFall();
        HandleAnimations();
    }

    void HandleMovement()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Aplicar fuerza para movimiento sensible, incluso con valores altos
        rb.AddForce(new Vector2(moveInput * moveAcceleration, 0));

        // Limitar velocidad horizontal a moveSpeed
        if (Mathf.Abs(rb.velocity.x) > moveSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * moveSpeed, rb.velocity.y);
        }

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

    void HandleBetterFall()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += new Vector2(0, Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }
    }

    void HandleAnimations()
    {
        // Solo caminar en el suelo
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
            jumpCount = 0; // Resetear saltos al tocar el suelo
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = false;
        }
    }
}