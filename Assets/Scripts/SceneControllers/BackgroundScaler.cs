using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        ScaleBackground();
    }

    void ScaleBackground()
    {
        if (spriteRenderer == null || mainCamera == null) return;

        // Obtener dimensiones del sprite y de la cámara
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Calcular escala necesaria
        float scaleX = cameraWidth / spriteWidth;
        float scaleY = cameraHeight / spriteHeight;

        // Aplicar escala (usar el mayor para cubrir toda la pantalla)
        float scale = Mathf.Max(scaleX, scaleY);
        transform.localScale = new Vector3(scale, scale, 1);

        // Posicionar en el centro de la cámara
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 10);
    }

    // Actualizar si la cámara cambia (opcional)
    void Update()
    {
        if (mainCamera.transform.hasChanged)
        {
            transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 10);
        }
    }
}
