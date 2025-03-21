using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkinChanger : MonoBehaviour {
    [SerializeField] private GachaMachine gachaMachine;
    [SerializeField] private SpriteRenderer playerSprite;
    private string currentSkin = "Default"; //initialize with default skin
   
    //When the game start, it will automatically make your skin be default.
    void Start(){
            ApplySkin(currentSkin);
    }
   
    //This is for the one for to apply the skin
    public void ApplySkin(string skinName){
        if (gachaMachine.mySkins.Contains(skinName)){
            playerSprite.sprite = gachaMachine.skinSprites[skinName];
            currentSkin = skinName;
        }
    }
   
    public string GetCurrentSkin(){
        return currentSkin;
    }
    // This is the button for changing the skin
    public void OnSkinButtonClicked(string skinName){
        ApplySkin(skinName);
    }
} 
