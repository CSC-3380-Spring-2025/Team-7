using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;     
using TMPro;             

public class GachaMachine : MonoBehaviour
{
    [Header("Skin Sprites")]
    public Sprite spriteDefault; 
    public Sprite WhiteCat;
    public Sprite BlackCat;
    public Sprite ShortCat;
    public Sprite SiameseCat;
    public Sprite CalicoCat;
    

    [Header("Skin Names By Rarity")]
    [SerializeField]
    private List<string> rareSkins = new List<string> { "WhiteCat", "BlackCat" }; 
    [SerializeField]
    private List<string> superRareSkins = new List<string> { "ShortCat", "SiameseCat" }; 
    [SerializeField]
    private List<string> ultraSkins = new List<string> { "CalicoCat" }; 

    public HashSet<string> mySkins = new HashSet<string>() { "Default" };

    public Dictionary<string, Sprite> skinSprites = new Dictionary<string, Sprite>();

    [Header("Visual Symbol Objects")]
    public GameObject pawsVisualObject;   
    public GameObject coinsVisualObject;  
    public GameObject snakeVisualObject; 
    public List<GameObject> noMatchVisualVariants; 

    [Header("Result Display")]
    [SerializeField] private TextMeshProUGUI resultText; 
    [SerializeField] private Image skinDisplay; 
    [SerializeField] private float winDisplayDelay = 1.5f; 

    // --- Initialization ---
    private void Awake()
    {
        InitializeSkinDictionary();
        HideAllVisuals();
        if (resultText != null) resultText.text = "";
        if (skinDisplay != null) skinDisplay.gameObject.SetActive(false);
    }

    private void InitializeSkinDictionary()
    {
        skinSprites.Clear(); 

        if (WhiteCat != null) skinSprites.Add("WhiteCat", WhiteCat); else Debug.LogError("SpriteRare1 not assigned!");
        if (BlackCat != null) skinSprites.Add("BlackCat", BlackCat); else Debug.LogError("SpriteRare2 not assigned!");
        if (ShortCat != null) skinSprites.Add("ShortCat", ShortCat); else Debug.LogError("SpriteSuperRare1 not assigned!");
        if (SiameseCat != null) skinSprites.Add("SiameseCat", SiameseCat); else Debug.LogError("SpriteSuperRare2 not assigned!");
        if (CalicoCat != null) skinSprites.Add("CalicoCat", CalicoCat); else Debug.LogError("SpriteUltraRare1 not assigned!");
    }

    // --- Hides all potential visual outcomes ---
    private void HideAllVisuals()
    {
        if (pawsVisualObject != null) pawsVisualObject.SetActive(false);
        if (coinsVisualObject != null) coinsVisualObject.SetActive(false);
        if (snakeVisualObject != null) snakeVisualObject.SetActive(false);

        if (noMatchVisualVariants != null)
        {
            foreach (GameObject variant in noMatchVisualVariants)
            {
                if (variant != null) variant.SetActive(false);
            }
        }
    }

    // --- Public function to start the sequence (call this from Button/Input) ---
    public void StartGachaSequence()
    {
        StopAllCoroutines();
        StartCoroutine(GachaRollCoroutine());
    }

    // --- The Main Gacha Coroutine ---
    private IEnumerator GachaRollCoroutine()
    {
        Debug.Log("Gacha Sequence Started...");

        HideAllVisuals();
        if (resultText != null) resultText.text = "";
        if (skinDisplay != null) skinDisplay.gameObject.SetActive(false);

        int roll = UnityEngine.Random.Range(1, 3001); // Range 1 to 3000

        GameObject visualToActivate = null;
        bool wonNewSkin = false;
        string debugOutcome = "Loss/Duplicate";
        string skinWonName = null; 

       
        // ULTRA RARE 1/30 chance
        if (roll <= 100)
        {
            debugOutcome = "Rolled Ultra Rare";
            int uRoll = UnityEngine.Random.Range(0, ultraSkins.Count);
            string selectedSkin = ultraSkins[uRoll];
            if (!mySkins.Contains(selectedSkin)) {
                mySkins.Add(selectedSkin);
                visualToActivate = snakeVisualObject;
                wonNewSkin = true;
                skinWonName = selectedSkin;
                debugOutcome = "NEW Ultra Rare Win!";
            } else { debugOutcome = "Duplicate Ultra Rare."; }
        }
        // SUPER RARE 1/25 chance
        else if (roll <= 100 + 120) // Up to 220
        {
            debugOutcome = "Rolled Super Rare";
            int srRoll = UnityEngine.Random.Range(0, superRareSkins.Count);
            string selectedSkin = superRareSkins[srRoll];
            if (!mySkins.Contains(selectedSkin)) {
                mySkins.Add(selectedSkin);
                visualToActivate = coinsVisualObject;
                wonNewSkin = true;
                skinWonName = selectedSkin;
                debugOutcome = "NEW Super Rare Win!";
            } else { debugOutcome = "Duplicate Super Rare."; }
        }
        // RARE 1/10 chance
        else if (roll <= 100 + 120 + 300) // Up to 520
        {
            debugOutcome = "Rolled Rare";
            int rRoll = UnityEngine.Random.Range(0, rareSkins.Count);
            string selectedSkin = rareSkins[rRoll];
            if (!mySkins.Contains(selectedSkin)) {
                mySkins.Add(selectedSkin);
                visualToActivate = pawsVisualObject;
                wonNewSkin = true;
                skinWonName = selectedSkin; 
                debugOutcome = "NEW Rare Win!";
            } else { debugOutcome = "Duplicate Rare."; }
        }
        // LOSS Range
        else {
            debugOutcome = "Loss (No Win).";
        }

        
        if (!wonNewSkin)
        {
            if (noMatchVisualVariants != null && noMatchVisualVariants.Count > 0) {
                int randomIndex = Random.Range(0, noMatchVisualVariants.Count);
                visualToActivate = noMatchVisualVariants[randomIndex];
            } else {
                Debug.LogError("No Match Visual Variants list is empty or not assigned!");
            }
        }

        Debug.Log($"Outcome: {debugOutcome}");

        if (visualToActivate != null)
        {
            Debug.Log($"Activating visual: {visualToActivate.name}");
            visualToActivate.SetActive(true);
        } else {
            Debug.LogWarning($"No visual object could be determined for outcome: {debugOutcome}");
        }

        yield return new WaitForSeconds(winDisplayDelay);

        Debug.Log("Displaying final result...");

        // Display Text
        if (resultText != null)
        {
            if (wonNewSkin && skinWonName != null) {
                resultText.text = $"You won {skinWonName}!";
            } else {
                resultText.text = "Try Again!";
            }
        } else { Debug.LogWarning("ResultText UI element not assigned!"); }

        if (wonNewSkin && skinWonName != null)
        {
            if (skinDisplay != null) {
                if (skinSprites.ContainsKey(skinWonName)) {
                    skinDisplay.sprite = skinSprites[skinWonName]; 
                    skinDisplay.gameObject.SetActive(true); 
                } else {
                    Debug.LogError($"Sprite key '{skinWonName}' not found in skinSprites dictionary! Cannot display sprite.");
                    skinDisplay.gameObject.SetActive(false);
                }
            } else { Debug.LogWarning("SkinDisplay Image UI element not assigned!"); }
        }
        else
        {
             if (skinDisplay != null) skinDisplay.gameObject.SetActive(false);
        }
        Debug.Log("Gacha Sequence Complete.");
    }
}