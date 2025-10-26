using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CongratulationsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text congratulationsText;
    public Text scoreText;
    public Button mainMenuButton;

    [Header("Música")]
    public AudioClip victoryMusic;
    public float musicVolume = 0.8f;

    [Header("Configuración")]
    public string mainMenuScene = "Juego";

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        PlayVictoryMusic();
        SetupButtons();
        UpdateUI();
    }

    void PlayVictoryMusic()
    {
        if (victoryMusic != null && audioSource != null)
        {
            audioSource.clip = victoryMusic;
            audioSource.volume = musicVolume;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void SetupButtons()
    {
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void UpdateUI()
    {
        if (congratulationsText != null)
            congratulationsText.text = "¡ENHORABUENA!";

        if (scoreText != null)
        {
            scoreText.text = $"Nivel Completado\nVida Final: {PlayerData.currentHealth}";
        }
    }

    public void GoToMainMenu()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        SceneManager.LoadScene(mainMenuScene);
    }
}