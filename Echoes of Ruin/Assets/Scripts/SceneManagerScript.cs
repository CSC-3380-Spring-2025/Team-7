using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {
    // References
    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
}
