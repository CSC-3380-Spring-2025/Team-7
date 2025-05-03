using UnityEngine;

public class Presists : MonoBehaviour
{
     private static bool instanceExists = false; // Prevent duplicates if scene reloads

    void Awake()
    {
        // Optional: Prevent duplicate instances if you reload the scene
        // where this object originates. If you only ever create it once,
        // you might not need this duplicate check.
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instanceExists = true; // Mark that an instance now persists
        Debug.Log($"PersistAcrossScenes: Called DontDestroyOnLoad on {gameObject.name}");
    }
}
