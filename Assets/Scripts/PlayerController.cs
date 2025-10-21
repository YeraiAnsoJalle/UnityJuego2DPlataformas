using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Configuraci�n del movimiento
    public float moveSpeed = 5f; // Velocidad del movimiento
    public float jumpForce = 10f; // Fuerza del salto
    private float moveInput; // Entrada horizontal (izquierda/derecha)
    private bool isGrounded; // Si est� tocando el suelo

    // Componentes del jugador
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim; // Referencia al Animator

    // Capa para comprobar si est� en el suelo
    public LayerMask groundMask;

    // Par�metros del Animator
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
        // Llamar a la funci�n para chequear si est� tocando el suelo
        isGrounded = Physics2D.IsTouchingLayers(coll, groundMask);

        // Llamar a las funciones de movimiento y salto
        HandleMovement();
        HandleJump();
        HandleAnimations();
    }

    void HandleMovement()
    {
        // Obtener la entrada del jugador (A/D o las flechas de direcci�n)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Movimiento horizontal
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void HandleJump()
    {
        // Si el jugador presiona la tecla de salto (por ejemplo, espacio) y est� tocando el suelo
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
            // El jugador se est� moviendo, activamos la animaci�n de caminar
            anim.SetBool(IsWalking, true);
        }
        else
        {
            // El jugador no se est� moviendo, desactivamos la animaci�n de caminar
            anim.SetBool(IsWalking, false);
        }

        // Controlar la animaci�n de salto
        if (!isGrounded)
        {
            // El jugador est� en el aire (saltando)
            anim.SetBool(IsJumping, true);
        }
        else
        {
            // El jugador est� tocando el suelo
            anim.SetBool(IsJumping, false);
        }
    }
}
