using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HealthPickupSound : MonoBehaviour
{
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        // Crear un tono simple de "curación"
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; // 2D sound

        // Configurar para el pickup
        audioSource.volume = 0.3f;
        audioSource.pitch = 1.2f;
    }

    // Llamar este método cuando recolectes
    public void PlayPickupSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        // Generar un clip simple programáticamente
        AudioClip clip = CreateToneClip(800, 0.3f); // Tono agudo
        audioSource.clip = clip;
        audioSource.Play();
    }

    private AudioClip CreateToneClip(float frequency, float duration)
    {
        int sampleRate = 44100;
        int samples = Mathf.CeilToInt(sampleRate * duration);
        float[] data = new float[samples];

        for (int i = 0; i < samples; i++)
        {
            data[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate);
            // Suavizar el final
            if (i > samples * 0.7f)
            {
                data[i] *= Mathf.Lerp(1, 0, (i - samples * 0.7f) / (samples * 0.3f));
            }
        }

        AudioClip clip = AudioClip.Create("PickupSound", samples, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }
}