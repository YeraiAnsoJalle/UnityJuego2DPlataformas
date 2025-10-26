using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    public Text gameOverText;
    public Text deathReasonText;

    void Start()
    {
        UpdateUI();
        Debug.Log("GameOverManager iniciado");
    }

    void UpdateUI()
    {
        if (gameOverText != null)
            gameOverText.text = "GAME OVER";

        if (deathReasonText != null)
            deathReasonText.text = "Has perdido en tu aventura";
    }

    public void RetryGame()
    {
        Debug.Log("Reiniciando juego...");

        PlayerData.currentHealth = PlayerData.maxHealth;
        BattleSessionData.defeatedEnemies.Clear();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameMusic();
        }

        SceneManager.LoadScene("Juego");
    }

    public void GoToMenu()
    {
        Debug.Log("Saliendo al menú...");
        SceneManager.LoadScene("Juego");
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}