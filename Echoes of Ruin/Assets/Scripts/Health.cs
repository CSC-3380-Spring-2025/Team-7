using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour

{
    [SerializeField] private int health = 100;

    private int MAX_HEALTH = 100;
    PlayerHealth player = new PlayerHealth();

    //Update is called once per frame
    void Update()
    {
      
    }

    public void Damage(int amount) {

        if (amount < 0){
            throw new System.ArgumentOutOfRangeException("Cannot have negative health");           
        }

        this.health -= amount;

        if(health <= 0){

           player.UpdateHP();
        } 
    }

    public void Heal(int amount) {

         if (amount < 0){
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");           
        }

        if(health + amount > MAX_HEALTH) {
            this.health = MAX_HEALTH;
        }
        else {

            this.health += amount;

        }
        
    }

  }
