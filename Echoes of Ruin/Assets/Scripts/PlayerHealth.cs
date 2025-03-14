using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{   public int playerHP;
    public GameObject[] hearts; //placeholder?

    private void Start()
    {   UpdateHP();
    }

    public void UpdateHP()
    {   if (playerHP <= 0)
        {   SceneManager.LoadScene("GameOver"); }
        else
        { for (int i = 0; i < hearts.Length; i++)
            { if (i < playerHP)
                { hearts[i] = Color.red; }
              else
                { hearts[i] = Color.black; }
            }
        }
    }

}
