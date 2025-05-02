using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviour

{
    [SerializeField] private int maxHearts = 5; // maximum number of hearts
    [SerializeField] public int currentHearts; // current hearts

    // PlayerHealth player = new PlayerHealth();
    PlayerHealth player;
    public int playerHP;
    public GameObject[] hearts;
    

    //Update is called once per frame
     void Start()
    {
        currentHearts = maxHearts;  // Start with full hearts
        player = GetComponent<PlayerHealth>();
          // Get PlayerHealth from the same GameObject
    }

   public void SetHearts(int maxHearts, int hearts)
    {
        this.maxHearts = maxHearts;
        this.currentHearts = hearts;
    }

   public void Damage(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
        }

//converting hp damage into number of hearts lost

        playerHP -=1;
        player.playerHP -=1;
        UpdateHP();
        player.UpdateHP();

        if (currentHearts <= 0)
        {
            currentHearts = 0;
            player.UpdateHP();  // Call when health is zero
        }
    }

   public void UpdateHP()
    {   
        if (playerHP <= 0){
          SceneManager.LoadScene("GameOver"); 
          Destroy(player);
          }else{
            for (int i = 0; i <hearts.Length; i++){
                
                hearts[i].SetActive(i < playerHP);
            }
          } 
        
    }
    public void Heal(int amount) {

         if (amount < 0){
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");           
        }

        if (currentHearts > maxHearts)
        {
            currentHearts = maxHearts;
        }
        else {
            playerHP += amount;
            playerHP = Mathf.Clamp(playerHP, 0, maxHearts);
            UpdateHP();
        }
        
    }

  }