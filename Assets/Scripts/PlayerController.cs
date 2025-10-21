using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Configuración del movimiento
    public float moveSpeed = 5f; // Velocidad del movimiento
    public float jumpForce = 10f; // Fuerza del salto
    private float moveInput; // Entrada horizontal (izquierda/derecha)
    private bool isGrounded; // Si está tocando el suelo

    // Componentes del jugador
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim; // Referencia al Animator

    // Capa para comprobar si está en el suelo
    public LayerMask groundMask;

    // Parámetros del Animator
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");


    // Start is called before the first frame update
    void Start()
    {
        // Obtener los componentes del jugador
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>(); // Obtener el componente Animator
    }

    // Update is called once per frame
    void Update()
    {
        // Llamar a la función para chequear si está tocando el suelo
        isGrounded = Physics2D.IsTouchingLayers(coll, groundMask);

        // Llamar a las funciones de movimiento y salto
        HandleMovement();
        HandleJump();
        HandleAnimations();
    }

    void HandleMovement()
    {
        // Obtener la entrada del jugador (A/D o las flechas de dirección)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Movimiento horizontal
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void HandleJump()
    {
        // Si el jugador presiona la tecla de salto (por ejemplo, espacio) y está tocando el suelo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Agregar una fuerza hacia arriba para hacer el salto
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void HandleAnimations()
    {
        // Controlar las animaciones de movimiento
        if (moveInput != 0)
        {
            // El jugador se está moviendo, activamos la animación de caminar
            anim.SetBool(IsWalking, true);
        }
        else
        {
            // El jugador no se está moviendo, desactivamos la animación de caminar
            anim.SetBool(IsWalking, false);
        }

        // Controlar la animación de salto
        if (!isGrounded)
        {
            // El jugador está en el aire (saltando)
            anim.SetBool(IsJumping, true);
        }
        else
        {
            // El jugador está tocando el suelo
            anim.SetBool(IsJumping, false);
        }
    }
}
