using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyEncounter : MonoBehaviour
{
    [Header("Datos del enemigo")]
    public string enemyName;
    public int maxHealth = 100;
    public int attackPower = 10;
    public string uniqueID;

    [Header("Escena de batalla")]
    public string battleSceneName = "BattleScene";

    private bool triggered = false;

    private void Start()
    {
        if (BattleSessionData.defeatedEnemies.Contains(uniqueID))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            PlayerPositionManager.lastPosition = other.transform.position;

            BattleData.enemyToLoad = new EnemyData(enemyName, maxHealth, attackPower, uniqueID);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayBattleMusic();
            }

            SceneManager.LoadScene(battleSceneName);
        }
    }
}