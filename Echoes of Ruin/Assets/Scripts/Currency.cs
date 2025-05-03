using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


//Currency Script
public class Currency : MonoBehaviour {

//public because we want it to be accesible to Item_Pickup
    public int coin;
    string sceneName;
    Scene scene;
//UI that displays amt to coins you have 

    public GameObject currencyUI;
    public TMP_Text CoinsTXT;

    //Finds game object thats called currency
    void Start() 
    {  
        
    }
    public void Update () 
    {   scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
        if (sceneName == "ForestClearing" || sceneName == "GachaScene" || sceneName == "TutorialScene")
        { currencyUI.SetActive(true);}
        else 
        { currencyUI.SetActive(false);}
        
        UpdateUI();
    }

    public void UpdateUI()  {
    if (coin < 0) coin = 0;

    if (CoinsTXT != null)
        CoinsTXT.text = "Coins: " + coin;
    }
}
