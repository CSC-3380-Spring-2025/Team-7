using UnityEngine;

public class LootPickup : MonoBehaviour {
    //Deletes item after pickup
     private void OnTriggerEnter2D(Collider2D player) {
        if (player.CompareTag("PlayerCat"))
        {
            Destroy(gameObject); 
        }
    }
}
