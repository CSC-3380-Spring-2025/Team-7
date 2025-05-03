using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{   public int playerHP;
    Scene scene;
    string sceneName;
    public GameObject healthUI;
    public GameObject[] hearts; //Array of heart GameObjects

    private void Start()
    { 
      scene = SceneManager.GetActiveScene();
      if (scene.name == "GameOver") {
        gameObject.SetActive(false); 
        return;
    }
      UpdateHP(); 
      }

    public void Update() 
    { scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
        if (sceneName == "TutorialScene")
        { healthUI.SetActive(true);}
        else if (sceneName != "ForestClearing")
        { healthUI.SetActive(false);
        }
      }

    public void UpdateHP()
    {  if (playerHP <= 0){
          SceneManager.LoadScene("GameOver");
          Audios.Instance.PlayMusic("GameOver"); 
          }
       else {
          for (int i = 0; i <hearts.Length; i++){
          hearts[i].SetActive(i < playerHP);
          }
        }
        
        
    }

}
