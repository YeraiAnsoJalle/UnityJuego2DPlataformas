using UnityEngine;

public class Spikes : MonoBehaviour
{
    [Header("Daño")]
    public int damage = 100;
    public float knockbackForce = 15f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HurtPlayer(collision.gameObject);
        }
    }

    private void HurtPlayer(GameObject playerObj)
    {
        PlayerController playerController = playerObj.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeEnvironmentalDamage(damage, "pinchos");

            ApplyKnockback(playerObj.transform);
        }
    }

    private void ApplyKnockback(Transform playerTransform)
    {
        Rigidbody2D playerRb = playerTransform.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 knockbackDirection = new Vector2(-Mathf.Sign(playerTransform.position.x - transform.position.x), 1f).normalized;
            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
}