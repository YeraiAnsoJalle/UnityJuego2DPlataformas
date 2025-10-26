using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : MonoBehaviour
{
    public static event Action OnSceneLoaded;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoadedCallback;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoadedCallback;
    }

    private void OnSceneLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoaded?.Invoke();
    }
}

