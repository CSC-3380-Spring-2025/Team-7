using System.Collections;
using System.Collectinos.Generic;
using UnityEngine;
using UnityEngine.UI;

//Currency Script
public class Currency : MonoBehavior {

//public because we want it to be accesible to Item_Pickup

    public int coin;

//UI that displays amt to coins you have 

    GameObject currencyUI;

    //Finds game object thats called currency
    void Start() 
    {
        currencyUI = GameObject.Find("Currency");
    }
    void Update () 
    {
        currencyUI.GetComponent<text>().text = coin.ToString();
        if (coin < 0)
        {
            coin = 0;
        }
    }
}
