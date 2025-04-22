using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//Currency Script
public class Currency : MonoBehaviour {

//public because we want it to be accesible to Item_Pickup

    public int coin;

//UI that displays amt to coins you have 

    GameObject currencyUI;
    public TMP_Text CoinsTXT;

    //Finds game object thats called currency
    void Start() 
    {
        currencyUI = GameObject.Find("Currency");
    }
    void Update () 
    {   //currencyUI.GetComponent<Text>().text = "Coins: " + coin.ToString();
        if (coin < 0)
        { coin = 0; }
        CoinsTXT.text = "Coins: " + coin.ToString();
    }
}
