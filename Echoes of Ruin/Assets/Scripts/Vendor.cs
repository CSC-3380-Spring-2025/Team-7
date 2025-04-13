using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Vendor : MonoBehaviour {

//takes refrence frpm currency script
    Currency script;

//allows for purchasing items including equipable items  
    public GameObject vendorUI;

    public int cost;
    public int[,] items = new int[4, 4];
    public TextMeshProUGUI CoinsTXT;

    private Canvas canvas;

//have to have an event system thats tagged gamecontroller where currency script is stored
    void Start() {
        script = GameObject.FindWithTag("GameController").GetComponent<Currency>();
        //item IDs
        items[1, 1] = 1;
        items[1, 2] = 2;
        items[1, 3] = 3;
        //item price
        items[2, 1] = 3;
        items[2, 2] = 3;
        items[2, 3] = 3;
        // item quantities
        items[3, 1] = 0;
        items[3, 2] = 0;
        items[3, 3] = 0;

        vendorUI.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D collider) {

        //makes vendor appear
        if (collider.gameObject.tag == "UI") {
            vendorUI.SetActive(true);
            Cursor.visible = true; 
        }
    }

    //Updaate is called once per frame
    void OnTriggerExit2D (Collider2D collider) {
       if (collider.gameObject.tag == "UI") {
            vendorUI.SetActive(false);
            Cursor.visible = false; 
        }
    } 

    public void BuyItem(){
        GameObject ShopButton = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        
        cost = items[2, ShopButton.GetComponent<ButtonInfo>().ItemID];
        if (script.coin >= cost) {
            script.coin -= cost;
            items[3, ShopButton.GetComponent<ButtonInfo>().ItemID]++;
            CoinsTXT.text = "Coins: " + script.coin.ToString();
            ShopButton.GetComponent<ButtonInfo>().QuantTXT.text = items[3, ShopButton.GetComponent<ButtonInfo>().ItemID].ToString();
        }
    }

}
