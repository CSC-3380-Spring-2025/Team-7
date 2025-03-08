using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//If collider has a health then they will recieve damage upon attack
public class AttackArea : MonoBehaviour 
{
   private int damage = 3;

   private void OnTriggerEnter2D(Collider2D collider)
   {
        if(collider.GetComponent<Health>() != null)
        {
            Health health = collider.GetComponent<Health>();
            health.Damage(damage) 
        }  
   }
}
