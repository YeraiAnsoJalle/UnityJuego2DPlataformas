using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Configuración de curación")]
    public int healAmount = 30;
    public string uniqueID = "";

    [Header("Efectos")]
    public GameObject pickupEffectPrefab;
    public AudioClip pickupSound;

    private bool collected = false;

    private void Start()
    {
        if (!string.IsNullOrEmpty(uniqueID) && HealthPickupManager.IsPickupCollected(uniqueID))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject playerObj)
    {
        collected = true;

        // Buscar el componente Character de diferentes maneras
        Character playerCharacter = playerObj.GetComponent<Character>();

        if (playerCharacter == null)
        {
            // Intentar buscar en hijos
            playerCharacter = playerObj.GetComponentInChildren<Character>();
        }

        if (playerCharacter == null)
        {
            Debug.LogError("No se encontró el componente Character en el jugador");
            return;
        }

        // Curar al jugador
        int healthBefore = playerCharacter.currentHealth;
        playerCharacter.Heal(healAmount);
        int actualHeal = playerCharacter.currentHealth - healthBefore;

        // Actualizar PlayerData
        PlayerData.currentHealth = playerCharacter.currentHealth;

        Debug.Log($"Jugador curado: {healthBefore} → {playerCharacter.currentHealth} (+{actualHeal} HP)");

        // Efecto visual
        if (pickupEffectPrefab != null)
        {
            Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            CreateSimpleEffect();
        }

        // Sonido
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
        else
        {
            PlaySimpleSound();
        }

        // Guardar en manager si tiene ID único
        if (!string.IsNullOrEmpty(uniqueID))
        {
            HealthPickupManager.RegisterPickupCollected(uniqueID);
        }

        // Ocultar y destruir el pickup
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Si tiene hijos (como iconos), también ocultarlos
        foreach (Transform child in transform)
        {
            SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
            if (childRenderer != null)
                childRenderer.enabled = false;
        }

        Destroy(gameObject, 2f);
    }

    private void CreateSimpleEffect()
    {
        GameObject effect = new GameObject("SimpleHealEffect");
        effect.transform.position = transform.position;

        // Crear partículas simples
        ParticleSystem ps = effect.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = Color.green;
        main.startSpeed = 2f;
        main.startLifetime = 1f;
        main.maxParticles = 10;

        ps.Play();
        Destroy(effect, 2f);
    }

    private void PlaySimpleSound()
    {
        GameObject soundObj = new GameObject("PickupSound");
        soundObj.transform.position = transform.position;
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        audioSource.volume = 0.3f;
        audioSource.Play();
        Destroy(soundObj, 2f);
    }
}