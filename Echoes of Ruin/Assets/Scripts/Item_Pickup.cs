using UnityEngine;

//Player currency will go up when picking up a coin and the currency will disappear once collected
public class Item_Pickup : MonoBehaviour{
    Currency script;

    // public int addAmount;

    void Start() {
        script = GameObject.FindWithTag("GameController").GetComponent<Currency>();
    } 

//Checks if player comes in contact with coin
//Accesses currecy scipt and accessed int gold to increase value
    void OnTriggerEnter2D(Collider2D obj) {
        if (obj.gameObject.tag == "Player")
        {
            //increments by one only
            script.coin ++;
            // = addAmount;

            Destroy(gameObject);    
        }
    }
}