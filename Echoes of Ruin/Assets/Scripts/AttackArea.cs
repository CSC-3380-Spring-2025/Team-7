using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//If collider has a health then they will recieve damage upon attack
public class AttackArea : MonoBehaviour 
{
   private int damage;

   private void OnTriggerEnter2D(Collider2D collider)
   {
        if(collider.GetComponent<Health>() != null)
        {
            Health health = collider.GetComponent<Health>();
            health.Damage(damage);

        }  
   }

//allows attack value to be dynamic
   public void SetDamage(int newDamage){
    
    damage = newDamage;
   } 
}
