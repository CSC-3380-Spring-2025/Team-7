using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    
    [SerializeField] private GachaMachine gachaMachine;
    [SerializeField] private SpriteRenderer playerSprite;

    private string currentSkin = "Default"; //initialize with default skin

    void Start()
    {
        // Apply the initial skin visually when the game starts
        ApplySkinVisuals(currentSkin);
    }

    public bool ApplySkinVisuals(string skinName)
    {
        // Check if the GachaMachine reference exists and if the skin is owned OR is the default skin
        if (gachaMachine != null && (skinName == "Default" || gachaMachine.mySkins.Contains(skinName)))
        {
            // Check if the sprite exists in the dictionary
            if (gachaMachine.skinSprites.ContainsKey(skinName))
            {
                playerSprite.sprite = gachaMachine.skinSprites[skinName];
                // Update the currentSkin variable ONLY if visuals are successfully applied
                this.currentSkin = skinName;
                Debug.Log($"Applied skin visuals: {skinName}");
                return true; // Success
            }
            else
            {
                Debug.LogWarning($"Skin '{skinName}' exists in mySkins but not in skinSprites dictionary!");
                return false; // Failed sprite missing
            }
        }
        else if (gachaMachine == null)
        {
             Debug.LogError("GachaMachine reference not set in SkinChanger!");
             return false; // Failed missing reference
        }
        else
        {
            Debug.LogWarning($"Attempted to apply skin '{skinName}' but it is not owned.");
            return false; // skin not owned
        }
    }

    public void OnSkinButtonClicked(string skinName)
    {
        ApplySkinVisuals(skinName);
    }

    public string GetCurrentSkinName()
    {
        return currentSkin;
    }

    public void SetCurrentSkin(string skinName)
    {
        if (!ApplySkinVisuals(skinName))
        {
             Debug.LogWarning($"Failed to apply loaded skin '{skinName}'. Reverting visuals to Default.");
             ApplySkinVisuals("Default");
        }
    }
}