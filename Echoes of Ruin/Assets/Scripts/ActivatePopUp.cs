using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivatePopUp : MonoBehaviour
{   public GameObject popup;
    public GameObject instructions;
    public bool test = false;
    public bool key = false;
    
    void Start() {
        popup.SetActive(false);
        instructions.gameObject.SetActive(false);
    }

    void Update() {
    if (Input.GetKeyDown(KeyCode.E)){
        key = !key;
    }
    if (key == true && test == true)
        { popup.SetActive(true); }
    else
        { popup.SetActive(false);}
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
