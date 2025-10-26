using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyEncounter : MonoBehaviour
{
    [Header("Datos del enemigo")]
    public string enemyName;
    public int maxHealth = 100;
    public int attackPower = 10;
    public string uniqueID; // Cada enemigo debe tener un ID único (por ejemplo: "enemy_001")

    [Header("Escena de batalla")]
    public string battleSceneName = "BattleScene";

    private bool triggered = false;

    private void Start()
    {
        // ✅ Si el enemigo fue derrotado en esta sesión, desaparece
        if (BattleSessionData.defeatedEnemies.Contains(uniqueID))
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            // ✅ Guardar posición del jugador antes de entrar a combate
            PlayerPositionManager.lastPosition = other.transform.position;

            // Guardar datos del enemigo para la batalla
            BattleData.enemyToLoad = new EnemyData(enemyName, maxHealth, attackPower, uniqueID);

            // Cambiar a la escena de batalla
            SceneManager.LoadScene(battleSceneName);
        }
    }
}

