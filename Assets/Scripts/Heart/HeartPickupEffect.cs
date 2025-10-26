using UnityEngine;

public class HealthPickupEffect : MonoBehaviour
{
    public ParticleSystem healParticles;
    public Light healLight;
    public float effectDuration = 1.5f;

    void Start()
    {
        // Configurar partículas si no las tienes
        if (healParticles == null)
        {
            CreateParticleSystem();
        }

        PlayEffect();
        Destroy(gameObject, effectDuration);
    }

    void CreateParticleSystem()
    {
        // Crear sistema de partículas programáticamente
        GameObject particlesObj = new GameObject("HealParticles");
        particlesObj.transform.SetParent(transform);
        particlesObj.transform.localPosition = Vector3.zero;

        healParticles = particlesObj.AddComponent<ParticleSystem>();
        var main = healParticles.main;
        var emission = healParticles.emission;
        var shape = healParticles.shape;
        var colorOverLifetime = healParticles.colorOverLifetime;

        // Configurar partículas
        main.startSpeed = 2f;
        main.startLifetime = 1f;
        main.startSize = 0.2f;
        main.startColor = new ParticleSystem.MinMaxGradient(
            new Color(0.2f, 0.8f, 0.2f), // Verde oscuro
            new Color(0.8f, 1.0f, 0.8f)  // Verde claro
        );
        main.maxParticles = 20;

        emission.rateOverTime = 15f;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.5f;
    }

    void PlayEffect()
    {
        if (healParticles != null)
        {
            healParticles.Play();
        }

        // Crear luz de curación
        GameObject lightObj = new GameObject("HealLight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = Vector3.zero;

        healLight = lightObj.AddComponent<Light>();
        healLight.color = Color.green;
        healLight.intensity = 2f;
        healLight.range = 3f;

        // Hacer que la luz se desvanezca
        StartCoroutine(FadeLight());
    }

    private System.Collections.IEnumerator FadeLight()
    {
        float timer = 0f;
        float startIntensity = healLight.intensity;

        while (timer < effectDuration)
        {
            timer += Time.deltaTime;
            healLight.intensity = Mathf.Lerp(startIntensity, 0f, timer / effectDuration);
            yield return null;
        }
    }
}
