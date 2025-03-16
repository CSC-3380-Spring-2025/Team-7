using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{   public int playerHP;
    public GameObject[] hearts; //Array of heart GameObjects

    private void Start()
    {   UpdateHP();
    }

    public void UpdateHP()
    {   if (playerHP <= 0){
          SceneManager.LoadScene("GameOver"); 
          }else{
            for (int i =0; i <hearts.Length; i++){
                hearts[i].SetActive(i < playerHP);
            }
          }
        
        
    }

}
