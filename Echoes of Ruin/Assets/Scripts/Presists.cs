using UnityEngine;

public class Presists : MonoBehaviour
{
     private static bool instanceExists = false; 

    void Awake()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instanceExists = true; 
    }
}
