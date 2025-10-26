using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagController : MonoBehaviour
{
    [Header("Configuración")]
    public string congratulationsScene = "Congratulations";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Bandera alcanzada! Nivel completado.");
            SceneManager.LoadScene(congratulationsScene);
        }
    }
}