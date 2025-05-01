using UnityEngine;

public class PersistObject : MonoBehaviour
{
    private static bool instanceExists = false;
    void Awake()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }
        instanceExists = true;
        DontDestroyOnLoad(gameObject);
    }
}