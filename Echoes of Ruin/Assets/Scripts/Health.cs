using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour

{
    [SerializeField] private int maxHearts = 5; // maximum number of hearts
    [SerializeField] private int heartValue =20; // hp per heart
    [SerializeField] private int currentHearts; // current hearts

    // PlayerHealth player = new PlayerHealth();
    PlayerHealth player;

    //Update is called once per frame
     void Start()
    {
        currentHearts = maxHearts;  // Start with full hearts
        player = GetComponent<PlayerHealth>();  // Get PlayerHealth from the same GameObject
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
        int heartsToLose = amount / heartValue;
        currentHearts -= heartsToLose;

        if (currentHearts <= 0)
        {
            currentHearts = 0;
            player.UpdateHP();  // Call when health is zero
        }
    }

    public void Heal(int amount) {

         if (amount < 0){
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");           
        }

         int heartsToGain = amount / heartValue;
        currentHearts += heartsToGain;

        if (currentHearts > maxHearts)
        {
            currentHearts = maxHearts;

        }
        
    }

  }
