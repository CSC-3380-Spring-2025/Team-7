using UnityEngine;

public class DontDestroyThisOnLoad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   for (int i = 0; i < Object.FindObjectsByType<DontDestroyThisOnLoad>(FindObjectsSortMode.InstanceID).Length; i++)
        {   if (Object.FindObjectsByType<DontDestroyThisOnLoad>(FindObjectsSortMode.InstanceID)[i] != this)
            {   if (Object.FindObjectsByType<DontDestroyThisOnLoad>(FindObjectsSortMode.InstanceID)[i].name == gameObject.name)
                {   Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
