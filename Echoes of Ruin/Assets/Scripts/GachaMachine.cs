using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Keep for Image
using TMPro;
using UnityEngine.SceneManagement;

public class GachaMachine : MonoBehaviour
{
    public static GachaMachine Instance { get; private set; } // Existing Singleton

    // --- Existing Variables ---
    public Sprite defaultSprite;
    public Sprite WhiteCat;
    public Sprite BlackCat;
    public Sprite ShortCat;
    public Sprite SiameseCat;
    public Sprite CalicoCat;

    [SerializeField] private List<string> rareSkins = new List<string> { "WhiteCat", "BlackCat" };
    [SerializeField] private List<string> superRareSkins = new List<string> { "ShortCat", "SiameseCat" };
    [SerializeField] private List<string> ultraSkins = new List<string> { "CalicoCat" };

    // This is the list that needs to be populated from the SaveLoadManager
    public HashSet<string> mySkins = new HashSet<string>() { "Default" };
    public Dictionary<string, Sprite> skinSprites = new Dictionary<string, Sprite>();

    // Scene specific references
    private GameObject pawsVisualObject;
    private GameObject coinsVisualObject;
    private GameObject snakeVisualObject;
    private List<GameObject> noMatchVisualVariants = new List<GameObject>();
    private TextMeshProUGUI resultText;
    private GameObject resultTextBackgroundObject;
    private Image skinDisplay;

    // Config
    private int GACHA_COST = 2;
    [SerializeField] private float winDisplayDelay = 1.0f;
    [SerializeField] private string gachaSceneName = "GachaScene"; // Scene where UI elements exist
    [SerializeField] private string uiParentName = "LeverScreen";
    [SerializeField] private string visualParentName = "LeverScreen";
    [SerializeField] private string resultBackgroundObjectName = "ResultBackground";
    [SerializeField] private string resultTextObjectName = "ResultText";
    [SerializeField] private string skinDisplayObjectName = "SkinDisplay";
    [SerializeField] private string noMatchVisualTag = "NoMatchVisual";
    // --- End Existing Variables ---

    // --- ADDED ---
    // Flag to ensure skins are only applied once from load data
    private bool _skinsAppliedFromLoad = false;
    // --- END ADDED ---

    private void Awake()
    {
        // --- Existing Singleton & DontDestroyOnLoad ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // --- End Existing ---

        InitializeSkinDictionary();
        SceneManager.sceneLoaded += OnSceneLoaded;

        // --- ADDED ---
        // Try to apply skins immediately in case Load happened before Awake
        TryApplyLoadedSkins();
        // --- END ADDED ---
    }

    void OnDestroy() // Existing
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

     void OnEnable() // ADDED - Try applying when re-enabled
    {
         TryApplyLoadedSkins();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // Existing
    {
        if (scene.name == gachaSceneName)
        {
            FindSceneReferences(); // Find UI elements specific to this scene
            ResetGachaState();
            // --- ADDED ---
            // Also try applying skins when the target scene loads
            TryApplyLoadedSkins();
            // --- END ADDED ---
        }
        else
        {
            ClearSceneReferences(); // Clear refs when leaving the scene
        }
    }

    // --- ADDED ---
    // Method to attempt applying loaded skins from the SaveLoadManager
    public void TryApplyLoadedSkins() // Make public so Manager can call it too
    {
        // Only apply once
        if (_skinsAppliedFromLoad) return;

        // Find the SaveLoadManager (using Singleton is cleaner now)
        MongoDBSaveLoadManager saveLoadManager = MongoDBSaveLoadManager.Instance;

        if (saveLoadManager != null)
        {
            List<string> loadedSkins = saveLoadManager.GetLoadedOwnedSkins();

            // Check if the manager actually has loaded data available
            if (loadedSkins != null)
            {
                Debug.Log($"[GachaMachine] Found SaveLoadManager. Applying {loadedSkins.Count} loaded owned skins.");
                // IMPORTANT: Overwrite the existing set with the loaded data
                mySkins = new HashSet<string>(loadedSkins);

                // Ensure "Default" skin is always present if needed by game logic
                if (!mySkins.Contains("Default"))
                {
                    mySkins.Add("Default");
                     Debug.Log("[GachaMachine] Added missing 'Default' skin to loaded set.");
                }

                _skinsAppliedFromLoad = true; // Mark as applied
                 Debug.Log($"[GachaMachine] mySkins HashSet now contains {mySkins.Count} skins.");

                 // Optional: Update any UI specific to the gacha machine showing owned skins
                 // UpdateOwnedSkinDisplay();
            }
            else
            {
                 // This means LoadGame hasn't successfully run yet OR returned null skins
                 Debug.LogWarning("[GachaMachine] Found SaveLoadManager, but no loaded skins data was available (GetLoadedOwnedSkins returned null). Retaining default/current skins.");
                 // We don't change mySkins in this case
            }
        }
        else
        {
            // This shouldn't happen if both are DontDestroyOnLoad Singletons,
            // but good to have a warning.
            Debug.LogWarning("[GachaMachine] Could not find MongoDBSaveLoadManager Instance when trying to apply skins!");
        }
    }
    // --- END ADDED ---


    // --- Rest of the existing GachaMachine methods ---
    // FindSceneReferences, ClearSceneReferences, ResetGachaState,
    // InitializeSkinDictionary, HideAllVisuals, StartGachaSequence,
    // GachaRollCoroutine, HideResultMessageAfterDelay
    // (These methods remain unchanged)
    // --- Existing Code ... (Paste the rest of your GachaMachine methods here) ---

    void FindSceneReferences()
    {
        resultText = null;
        resultTextBackgroundObject = null;
        skinDisplay = null;
        pawsVisualObject = null;
        coinsVisualObject = null;
        snakeVisualObject = null;
        noMatchVisualVariants.Clear();

        GameObject uiParentObject = GameObject.Find(uiParentName);
        if (uiParentObject != null)
        {
            Transform uiParentTransform = uiParentObject.transform;

            Transform backgroundTransform = uiParentTransform.Find(resultBackgroundObjectName);
            if (backgroundTransform != null)
            {
                resultTextBackgroundObject = backgroundTransform.gameObject;
                Transform resultTextTransform = backgroundTransform.Find(resultTextObjectName);
                if (resultTextTransform != null)
                {
                    resultText = resultTextTransform.GetComponent<TextMeshProUGUI>();
                } else { Debug.LogWarning($"[GachaMachine] Result Text '{resultTextObjectName}' not found under '{resultBackgroundObjectName}'!"); }
            } else { Debug.LogWarning($"[GachaMachine] Result Background '{resultBackgroundObjectName}' not found under '{uiParentName}'!"); }

            Transform skinDisplayTransform = uiParentTransform.Find(skinDisplayObjectName);
            if (skinDisplayTransform != null)
            {
                skinDisplay = skinDisplayTransform.GetComponent<Image>();
            } else { Debug.LogWarning($"[GachaMachine] Skin Display '{skinDisplayObjectName}' not found under '{uiParentName}'!"); }
        } else { Debug.LogWarning($"[GachaMachine] UI Parent '{uiParentName}' not found!"); }

        GameObject visualParentObject = GameObject.Find(visualParentName);
        if (visualParentObject != null)
        {
            Transform visualParentTransform = visualParentObject.transform;

            Transform pawsTransform = visualParentTransform.Find("Paws");
            if (pawsTransform != null) pawsVisualObject = pawsTransform.gameObject; else { Debug.LogWarning("[GachaMachine] Paws visual not found!"); }

            Transform coinsTransform = visualParentTransform.Find("Coins");
            if (coinsTransform != null) coinsVisualObject = coinsTransform.gameObject; else { Debug.LogWarning("[GachaMachine] Coins visual not found!"); }

            Transform snakeTransform = visualParentTransform.Find("Snake");
            if (snakeTransform != null) snakeVisualObject = snakeTransform.gameObject; else { Debug.LogWarning("[GachaMachine] Snake visual not found!"); }

            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(noMatchVisualTag);
            int foundTagged = 0;
            foreach (GameObject taggedObj in taggedObjects)
            {
                // Ensure the tagged object is actually part of the expected visual parent
                if (taggedObj != null && taggedObj.transform.IsChildOf(visualParentTransform))
                {
                    noMatchVisualVariants.Add(taggedObj);
                    foundTagged++;
                }
            }
             Debug.Log($"[GachaMachine] Found {foundTagged} visual variants with tag '{noMatchVisualTag}'.");
        } else { Debug.LogWarning($"[GachaMachine] Visual Parent '{visualParentName}' not found!"); }

         // Check if essential references were found
         if(resultText == null) Debug.LogError("[GachaMachine] CRITICAL: resultText reference is missing after FindSceneReferences!");
         if(skinDisplay == null) Debug.LogError("[GachaMachine] CRITICAL: skinDisplay reference is missing after FindSceneReferences!");

    }


    void ClearSceneReferences()
    {
        pawsVisualObject = null;
        coinsVisualObject = null;
        snakeVisualObject = null;
        noMatchVisualVariants.Clear();
        resultText = null;
        resultTextBackgroundObject = null;
        skinDisplay = null;
    }


    private void ResetGachaState()
    {
        HideAllVisuals();

        if (resultTextBackgroundObject != null) { resultTextBackgroundObject.SetActive(false); }
        if (resultText != null) { resultText.text = ""; }
        if (skinDisplay != null) { skinDisplay.gameObject.SetActive(false); }
    }


    private void InitializeSkinDictionary()
    {
        skinSprites.Clear();
        if (defaultSprite != null) skinSprites.Add("Default", defaultSprite);
        if (WhiteCat != null) skinSprites.Add("WhiteCat", WhiteCat);
        if (BlackCat != null) skinSprites.Add("BlackCat", BlackCat);
        if (ShortCat != null) skinSprites.Add("ShortCat", ShortCat);
        if (SiameseCat != null) skinSprites.Add("SiameseCat", SiameseCat);
        if (CalicoCat != null) skinSprites.Add("CalicoCat", CalicoCat);
         Debug.Log($"[GachaMachine] Initialized Skin Dictionary with {skinSprites.Count} entries.");
    }


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


    public void StartGachaSequence()
    {
        // Ensure we are in the correct scene with valid references before proceeding
        if (resultText == null || skinDisplay == null)
        {
             Debug.LogError("[GachaMachine] Attempted StartGachaSequence but scene references (resultText/skinDisplay) are null. Are you in the correct scene ('" + gachaSceneName + "')?");
             // Optionally display an error to the player via a persistent UI element
             return;
        }

        Currency currencyComponent = null;
        GameObject currencyHolderObject = GameObject.Find("HeartsAndCoinsOverlay"); // Assuming this object also persists

        if (currencyHolderObject != null)
        {
            currencyComponent = currencyHolderObject.GetComponent<Currency>();
        }

        // Check currency component
        if (currencyComponent == null)
        {
             Debug.LogError("[GachaMachine] Currency component/object not found!");
             if (resultTextBackgroundObject != null) resultTextBackgroundObject.SetActive(true);
             if (resultText != null) resultText.text = "Error: Currency!";
             return;
        }

        // Check cost
        if (currencyComponent.coin >= GACHA_COST)
        {
            currencyComponent.coin -= GACHA_COST;
            // ADDED: Save game immediately after spending currency
            MongoDBSaveLoadManager.Instance?.SaveGame();
            // ---
            StartCoroutine(GachaRollCoroutine());
        }
        else // Not enough coins
        {
            if (resultTextBackgroundObject != null) { resultTextBackgroundObject.SetActive(true); }
            if (resultText != null) { resultText.text = $"Need {GACHA_COST} Coins!"; }
             // Optional: Hide message after delay
             // StartCoroutine(HideResultMessageAfterDelay(2.0f));
        }
    }


    private IEnumerator GachaRollCoroutine()
    {
        // Initial UI reset for the roll sequence
        HideAllVisuals();
        if (resultTextBackgroundObject != null) resultTextBackgroundObject.SetActive(false);
        if (skinDisplay != null) skinDisplay.gameObject.SetActive(false);

        // --- Roll Logic ---
        int roll = UnityEngine.Random.Range(1, 3001); // Example range
        GameObject visualToActivate = null;
        bool wonNewSkin = false;
        string skinWonName = null;
        string rarity = "Common"; // Default

        // Check Ultra Rare
        if (roll <= 100 && ultraSkins.Count > 0) {
            string selectedSkin = ultraSkins[UnityEngine.Random.Range(0, ultraSkins.Count)];
            if (mySkins.Add(selectedSkin)) // TryAdd returns true if added (new skin)
            {
                visualToActivate = snakeVisualObject; // Ultra rare visual
                wonNewSkin = true;
                skinWonName = selectedSkin;
                rarity = "Ultra Rare";
            }
        }
        // Check Super Rare (only if Ultra didn't win a *new* skin)
        else if (roll <= 220 && superRareSkins.Count > 0) {
            string selectedSkin = superRareSkins[UnityEngine.Random.Range(0, superRareSkins.Count)];
             if (mySkins.Add(selectedSkin))
            {
                visualToActivate = coinsVisualObject; // Super rare visual
                wonNewSkin = true;
                skinWonName = selectedSkin;
                 rarity = "Super Rare";
            }
        }
        // Check Rare (only if higher tiers didn't win a *new* skin)
        else if (roll <= 520 && rareSkins.Count > 0) {
             string selectedSkin = rareSkins[UnityEngine.Random.Range(0, rareSkins.Count)];
             if (mySkins.Add(selectedSkin))
            {
                visualToActivate = pawsVisualObject; // Rare visual
                wonNewSkin = true;
                skinWonName = selectedSkin;
                rarity = "Rare";
            }
        }

        // If no NEW skin was won from the rarity tiers, pick a common visual
        if (!wonNewSkin) {
             if (noMatchVisualVariants != null && noMatchVisualVariants.Count > 0) {
                visualToActivate = noMatchVisualVariants[UnityEngine.Random.Range(0, noMatchVisualVariants.Count)];
            } else { Debug.LogWarning("[GachaMachine] No 'NoMatchVisual' variants found or assigned!"); }
             rarity = "Common";
        }
        // --- End Roll Logic ---

        // Activate the chosen visual
        if (visualToActivate != null)
        {
            visualToActivate.SetActive(true);
        } else { Debug.LogWarning("[GachaMachine] No visual decided to activate."); }

        // Wait for visual display
        yield return new WaitForSeconds(winDisplayDelay);

        // Show Result Text Background
        if (resultTextBackgroundObject != null) { resultTextBackgroundObject.SetActive(true); }

        // Display Result Text
        if (resultText != null)
        {
            if (wonNewSkin && skinWonName != null)
            {
                resultText.text = $"[{rarity}]\nYou won {skinWonName}!";
                 // ADDED: Save game immediately after winning a new skin
                 MongoDBSaveLoadManager.Instance?.SaveGame();
                 // ---
            }
            else // Didn't win a NEW skin (either common or duplicate)
            {
                resultText.text = "Try Again!";
                 // Optional: Hide message after delay if it's just "Try Again"
                 // StartCoroutine(HideResultMessageAfterDelay(2.0f));
            }
        }

        // Display Skin Image if a new skin was won
        if (wonNewSkin && skinWonName != null)
        {
            if (skinDisplay != null)
            {
                if (skinSprites.TryGetValue(skinWonName, out Sprite spriteToShow))
                {
                    skinDisplay.sprite = spriteToShow;
                    skinDisplay.gameObject.SetActive(true); // Show the image display
                }
                else // Skin name exists but no sprite found in dictionary
                {
                    Debug.LogError($"[GachaMachine] Won skin '{skinWonName}' but no sprite found in skinSprites dictionary!");
                    skinDisplay.gameObject.SetActive(false);
                }
            }
        }
        else // Didn't win a new skin, ensure display is hidden
        {
            if (skinDisplay != null) skinDisplay.gameObject.SetActive(false);
        }
    }


    private IEnumerator HideResultMessageAfterDelay(float delay) // Existing helper
    {
        yield return new WaitForSeconds(delay);
        if (resultTextBackgroundObject != null)
        {
            resultTextBackgroundObject.SetActive(false);
        }
        if(skinDisplay != null && !wonNewSkin) // Also hide skin display if no new skin won
        {
           // This logic might be complex, ResetGachaState might be better if called from UI button
        }

    }

     // Helper variable used by coroutine - needs to be member if HideResultMessageAfterDelay uses it
     private bool wonNewSkin = false;

} // End of GachaMachine class