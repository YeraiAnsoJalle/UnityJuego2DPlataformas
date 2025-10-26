using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Botones")]
    public Button startButton;

    [Header("Sonido")]
    public AudioClip menuMusic;
    public AudioClip clickSound;

    [Header("Configuración")]
    public string gameScene = "Juego";

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        PlayMenuMusic();

        if (startButton != null)
            startButton.onClick.AddListener(StartGame);
    }

    void PlayMenuMusic()
    {
        if (menuMusic != null && audioSource != null)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true; 
            audioSource.volume = 0.7f;
            audioSource.Play();
            Debug.Log("Reproduciendo música del menú");
        }
    }

    void StartGame()
    {
        PlayClickSound();
        Debug.Log("Iniciando juego...");

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        SceneManager.LoadScene(gameScene);
    }

    void PlayClickSound()
    {
        if (clickSound != null)
        {
            GameObject soundObject = new GameObject("ClickSound");
            AudioSource clickAudio = soundObject.AddComponent<AudioSource>();
            clickAudio.PlayOneShot(clickSound);

            Destroy(soundObject, clickSound.length);
        }
    }
}