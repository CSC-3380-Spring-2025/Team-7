using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour{

     public GameObject MainMenu;
     private bool isVisible;

    // sets main menu to invisble upon loadup
    void Start(){
        if (MainMenu != null)
            MainMenu.SetActive(false);
            isVisible = false;
    }

    // Opens and closes main menu on escape button
    void Update(){
         if (Input.GetKeyDown(KeyCode.Escape)) {
             if (MainMenu != null){
                isVisible = !isVisible;
                MainMenu.SetActive(isVisible);
             }
        }
    }
}
