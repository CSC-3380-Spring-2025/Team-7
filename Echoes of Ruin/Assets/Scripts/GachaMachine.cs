using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;      
using TMPro;             

public class GachaMachine : MonoBehaviour
{
    
    [Header("Skin Sprites")]
    public Sprite spriteDefault; 
    public Sprite spriteRare1;
    public Sprite spriteRare2;
    public Sprite spriteSuperRare1;
    public Sprite spriteSuperRare2;
    public Sprite spriteUltraRare1;
    

    
    [Header("Skin Names By Rarity")]
    [SerializeField]
    private List<string> rareSkins = new List<string> { "Rare1", "Rare2" }; 
    [SerializeField]
    private List<string> superRareSkins = new List<string> { "SuperRare1", "SuperRare2" }; 
    [SerializeField]
    private List<string> ultraSkins = new List<string> { "UltraRare1" }; 
  
    public HashSet<string> mySkins = new HashSet<string>() { "Default" }; 
    
    public Dictionary<string, Sprite> skinSprites = new Dictionary<string, Sprite>();

   
    [Header("Visual Symbol Objects")]
    public GameObject pawsVisualObject;  
    public GameObject coinsVisualObject;  
    public GameObject snakeVisualObject;  
    public GameObject noMatchVisualObject; 

    
    private void Awake()
    {
        InitializeSkinDictionary();
        HideAllVisuals(); 
    }

    private void InitializeSkinDictionary()
    {
       
        skinSprites.Clear();

        skinSprites.Add("Rare1", spriteRare1);
        skinSprites.Add("Rare2", spriteRare2);
        skinSprites.Add("SuperRare1", spriteSuperRare1);
        skinSprites.Add("SuperRare2", spriteSuperRare2);
        skinSprites.Add("UltraRare1", spriteUltraRare1);
    }

    private void HideAllVisuals()
    {
        if (pawsVisualObject != null) pawsVisualObject.SetActive(false);
        if (coinsVisualObject != null) coinsVisualObject.SetActive(false);
        if (snakeVisualObject != null) snakeVisualObject.SetActive(false);
        if (noMatchVisualObject != null) noMatchVisualObject.SetActive(false);
    }

    public void StartGachaSequence()
    {
        
        StopAllCoroutines();
        StartCoroutine(GachaRollCoroutine());
    }

   
    private IEnumerator GachaRollCoroutine()
    {
        Debug.Log("Gacha Sequence Started...");

        
        HideAllVisuals();
        // if (resultText != null) resultText.text = ""; // Hide previous text if added
        // if (skinDisplay != null) skinDisplay.gameObject.SetActive(false); // Hide previous skin if added

        // --- Optional: Trigger spinning animations here ---
        // PlaySpinAnimation(); 
        // yield return new WaitForSeconds(spinDuration); // Wait for spinning
  
        int roll = UnityEngine.Random.Range(1, 2097153);
 
        GameObject visualToActivate = noMatchVisualObject; 
        bool wonNewSkin = false;
        string debugOutcome = "Loss/Duplicate";
        // string skinWonName = ""; // To store for future display

    
        
        if (roll <= 8192) 
        {
            int uRoll = UnityEngine.Random.Range(0, ultraSkins.Count);
            string selectedSkin = ultraSkins[uRoll];
            if (!mySkins.Contains(selectedSkin)) 
            {
                mySkins.Add(selectedSkin);          
                visualToActivate = snakeVisualObject; 
                wonNewSkin = true;
                debugOutcome = "NEW Ultra Rare Win!";
                // skinWonName = selectedSkin;
            } else { debugOutcome = "Duplicate Ultra Rare."; } 
        }
        
        else if (roll <= 8192 + 16384) 
        {
            int srRoll = UnityEngine.Random.Range(0, superRareSkins.Count);
            string selectedSkin = superRareSkins[srRoll];
            if (!mySkins.Contains(selectedSkin)) 
            {
                mySkins.Add(selectedSkin);
                visualToActivate = coinsVisualObject; 
                wonNewSkin = true;
                debugOutcome = "NEW Super Rare Win!";
                // skinWonName = selectedSkin;
            } else { debugOutcome = "Duplicate Super Rare."; } 
        }
        
        else if (roll <= 8192 + 16384 + 32768) 
        {
            int rRoll = UnityEngine.Random.Range(0, rareSkins.Count);
            string selectedSkin = rareSkins[rRoll];
            if (!mySkins.Contains(selectedSkin)) 
            {
                mySkins.Add(selectedSkin);
                visualToActivate = pawsVisualObject;  
                wonNewSkin = true;
                debugOutcome = "NEW Rare Win!";
                // skinWonName = selectedSkin;
            } else { debugOutcome = "Duplicate Rare."; } 
        }
        
        else
        {
            debugOutcome = "Loss (No Win).";
           
        }

        Debug.Log($"Outcome: {debugOutcome}");

        
        if (visualToActivate != null)
        {
            Debug.Log($"Activating visual: {visualToActivate.name}");
            visualToActivate.SetActive(true);
            // --- Optional: Trigger a "Landing" animation on the specific visual ---
            // Animator visualAnimator = visualToActivate.GetComponent<Animator>();
            // if (visualAnimator != null) visualAnimator.SetTrigger("Land");
        }
        else
        {
            Debug.LogWarning("The visual object to activate is not assigned or is null!");
        }

        // --- FUTURE: Display specific skin/text after a delay ---
        // if (wonNewSkin) {
        //    yield return new WaitForSeconds(symbolDisplayDelay);
        //    if (resultText != null) { /* Set Win Text */ }
        //    if (skinDisplay != null) { /* Set Sprite & Activate */ }
        // } else {
        //    yield return new WaitForSeconds(symbolDisplayDelay * 0.5f); // Shorter delay for loss?
        //    if (resultText != null) { /* Set Try Again Text */ }
        // }

        yield return null;
    }
}