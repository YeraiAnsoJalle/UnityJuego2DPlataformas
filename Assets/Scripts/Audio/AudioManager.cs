using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Música de fondo")]
    public AudioClip gameMusic;
    public AudioClip battleMusic;

    private AudioSource musicSource;
    private string currentScene;

    void Awake()
    {
        // Patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Crear AudioSource para música
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = 0.7f;

        // Suscribirse al evento de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cambiar música según la escena
        if (scene.name == "Juego")
        {
            PlayGameMusic();
        }
        else if (scene.name == "BattleScene")
        {
            PlayBattleMusic();
        }
        else if (scene.name == "GameOver")
        {
            // Opcional: parar música en GameOver o poner tema triste
            musicSource.Stop();
        }
    }

    public void PlayGameMusic()
    {
        if (musicSource.clip != gameMusic || !musicSource.isPlaying)
        {
            musicSource.clip = gameMusic;
            musicSource.Play();
            Debug.Log("Reproduciendo música del juego");
        }
    }

    public void PlayBattleMusic()
    {
        if (musicSource.clip != battleMusic || !musicSource.isPlaying)
        {
            musicSource.clip = battleMusic;
            musicSource.Play();
            Debug.Log("Reproduciendo música de batalla");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    void OnDestroy()
    {
        // Desuscribirse del evento
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
