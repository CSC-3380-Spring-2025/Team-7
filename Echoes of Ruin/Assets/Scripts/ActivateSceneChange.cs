using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ActivateSceneChange : MonoBehaviour
{   public GameObject instructions;
    public string sceneName = "";
    public bool test = false;
    [SerializeField]private SceneManagerScript manag;
    
    void Start() {
       // manag =  GameObject.Find("Change").GetComponent<SceneManagerScript>();
        instructions.gameObject.SetActive(false);
    }

    void Update() {
    if (Input.GetKeyDown(KeyCode.E) && test == true)
        { manag.LoadScene(sceneName); }
  }

  void OnTriggerEnter2D(Collider2D collide) {
    instructions.gameObject.SetActive(true);
    test = true;
  }
  void OnTriggerExit2D(Collider2D collide) {
    instructions.gameObject.SetActive(false);
    test = false;
  }
}
