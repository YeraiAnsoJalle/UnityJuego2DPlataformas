using UnityEngine;

public class Spikes : MonoBehaviour
{
    [Header("Daño")]
    public int damage = 30;
    public float knockbackForce = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HurtPlayer(collision.gameObject);
        }
    }

    private void HurtPlayer(GameObject playerObj)
    {
        // Obtener componente Character del jugador
        Character player = playerObj.GetComponent<Character>();
        if (player != null)
        {
            // Aplicar daño
            player.TakeDamage(damage);

            // Actualizar PlayerData
            PlayerData.currentHealth = player.currentHealth;

            Debug.Log($"¡Spikes! Jugador pierde {damage} de vida");

            // Aplicar knockback
            ApplyKnockback(playerObj.transform);
        }
    }

    private void ApplyKnockback(Transform playerTransform)
    {
        Rigidbody2D playerRb = playerTransform.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // Calcular dirección del knockback (hacia arriba)
            Vector2 knockbackDirection = new Vector2(0, 1).normalized;

            // Aplicar fuerza de knockback
            playerRb.velocity = Vector2.zero; // Resetear velocidad primero
            playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            Debug.Log("Knockback aplicado al jugador");
        }
    }
}
