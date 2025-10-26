using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("M�sica de fondo")]
    public AudioClip gameMusic;
    public AudioClip battleMusic;

    private AudioSource musicSource;
    private string currentScene;

    void Awake()
    {
        // Patr�n Singleton
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

        // Crear AudioSource para m�sica
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = 0.7f;

        // Suscribirse al evento de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cambiar m�sica seg�n la escena
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
            // Opcional: parar m�sica en GameOver o poner tema triste
            musicSource.Stop();
        }
    }

    public void PlayGameMusic()
    {
        if (musicSource.clip != gameMusic || !musicSource.isPlaying)
        {
            musicSource.clip = gameMusic;
            musicSource.Play();
            Debug.Log("Reproduciendo m�sica del juego");
        }
    }

    public void PlayBattleMusic()
    {
        if (musicSource.clip != battleMusic || !musicSource.isPlaying)
        {
            musicSource.clip = battleMusic;
            musicSource.Play();
            Debug.Log("Reproduciendo m�sica de batalla");
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
