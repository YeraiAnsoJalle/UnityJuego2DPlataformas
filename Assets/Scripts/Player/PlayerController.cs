using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 60f;
    public float jumpForce = 8f;

    [Header("Componentes")]
    public Animator anim;

    [Header("Muerte")]
    public float killYPosition = -30f;

    private float moveInput;
    private bool isGrounded;
    private int jumpCount;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (anim == null)
            anim = GetComponentInChildren<Animator>();

        jumpCount = 0;
    }

    void Update()
    {
        HandleInput();
        HandleJump();
        HandleAnimations();
        CheckDeathByFall();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
    }

    void HandleMovement()
    {
        Vector2 movement = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }
    }

    void HandleAnimations()
    {
        if (anim != null)
        {
            bool walking = moveInput != 0 && isGrounded;
            bool jumping = !isGrounded;

            anim.SetBool("IsWalking", walking);
            anim.SetBool("IsJumping", jumping);
        }
    }

    void CheckDeathByFall()
    {
        if (transform.position.y < killYPosition)
        {
            Debug.Log("¡Jugador cayó al vacío!");
            Die("Has caído al vacío");
        }
    }

    public void Die(string deathReason = "Has muerto")
    {
        Debug.Log($"Muerte del jugador: {deathReason}");
        PlayerData.currentHealth = 0;
        SceneManager.LoadScene("GameOver");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("InstantDeath"))
        {
            Debug.Log("¡Muerte instantánea!");
            Die("Muerte instantánea");
        }
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

    public void TakeEnvironmentalDamage(int damage, string damageSource = "peligro ambiental")
    {
        if (PlayerData.currentHealth > 0)
        {
            PlayerData.currentHealth -= damage;
            if (PlayerData.currentHealth < 0)
                PlayerData.currentHealth = 0;

            Debug.Log($"Daño de {damageSource}: -{damage} HP. Vida restante: {PlayerData.currentHealth}");

            if (PlayerData.currentHealth <= 0)
            {
                Die($"Muerto por {damageSource}");
            }
        }
    }
}