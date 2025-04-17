using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivatePopUp : MonoBehaviour
{   public GameObject popup;
    
    void Update() {
    if (Input.GetKeyDown("e"))
        { popup.SetActive(true); }
    else
        { popup.SetActive(false);}
  }
}
