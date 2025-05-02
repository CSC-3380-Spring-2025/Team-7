using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LootPickup : MonoBehaviour {
    public Currency Currency;
    public Health Health;
    public GameObject HeartsCoinsUI; 


    void Start() {
        HeartsCoinsUI = GameObject.FindGameObjectWithTag("HeartsCoins");
        if (HeartsCoinsUI != null) {
            Currency = HeartsCoinsUI.GetComponent<Currency>();
            Health = HeartsCoinsUI.GetComponent<Health>();
        }
    }

    //Deletes item after pickup & refills hearts and counts coins
     private void OnTriggerEnter2D(Collider2D player) {
        if (player.CompareTag("PlayerCat")) {

            if(gameObject.name.Contains("Coin")) {
                Currency.coin++;
                Currency.CoinsTXT.text = "Coins: " + Currency.coin;
                Currency.UpdateUI();
                Destroy(gameObject); 
            }
            
            if(gameObject.name.Contains("Heart")) {
                Health.Heal(1);
                Destroy(gameObject); 
            }
            Destroy(gameObject); 
            
        }
    }
}
