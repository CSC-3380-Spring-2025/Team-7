using System.Collections;
using Systems.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour {

//takes refrence frpm currency script
    Currency script;

//allows for purchasing items including equipable items  
    public GameObject vendorUI;
    public GameObject objToCreate;
    public Transform posToCreate;

    public int cost;

//have to have an event system thats tagged gamecontroller where currency script is stored
    void Start() {
        script = GameObject.FindWithTag("GameController").GetComponent<Currency>();
    }
    void OnTriggerEnter() {

        //makes vendor appear
        vendorUI.SetActive(true);
        Cursor.visible = true;  
    }

    //Updaate is called once per frame
    void OnTriggerExit () {

        vendorUI.SetActive(false);
        Cursor.visible = false;

    } 

    public void BuyItem(){

        if (script.coin >= cost){

            script.coin -= cost;
            Instantiate(objToCreate, posToCreate.position,posToCreate.rotation);
        }
    }

}