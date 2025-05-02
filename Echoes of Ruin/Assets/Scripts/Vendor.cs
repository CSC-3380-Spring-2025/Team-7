using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class Vendor : MonoBehaviour {

//takes refrence frpm currency script
    Currency script;
    ItemTrack track;

//allows for purchasing items including equipable items  
    public GameObject vendorUI;
    public GameObject noMoney;

    public int cost;
    public int ID;
    public int[,] items = new int[4, 4];

//have to have an event system thats tagged gamecontroller where currency script is stored
    void Start() {
        script = GameObject.Find("HeartsAndCoinsOverlay").GetComponent<Currency>();
        track = GameObject.Find("HeartsAndCoinsOverlay").GetComponent<ItemTrack>();

        noMoney.SetActive(false);

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
    }
    void OnTriggerEnter() {

        //makes vendor appear
        vendorUI.SetActive(true);
        Cursor.visible = true;  
        noMoney.SetActive(false);
    }

    //Updaate is called once per frame
    void OnTriggerExit () {

        vendorUI.SetActive(false);
        Cursor.visible = false;
        noMoney.SetActive(false);
    } 

    public void BuyItem(){
        GameObject ShopButton = GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject;
        ID = ShopButton.GetComponent<ButtonInfo>().ItemID;
        cost = items[2, ID];
        if (script.coin >= cost) {
            script.coin -= cost;
            items[3, ID]++;
            switch(ID)
            { case(1):
                track.ball++;
                break;
              case(2):
                track.bisc++;
                break;
              case(3):
                track.brush++;
                break;
            }
            noMoney.SetActive(false);
        }
        else if (script.coin < cost) {
            noMoney.SetActive(true);
        }
    }

}
