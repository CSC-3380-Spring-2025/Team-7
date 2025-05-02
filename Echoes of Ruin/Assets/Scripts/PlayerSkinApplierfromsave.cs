using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management
using System.Collections; // Needed for Coroutines (IEnumerator)

// Attach this script to the SAME GameObject/Prefab as the original Player.cs script (e.g., PlayerCat)
public class PlayerSkinApplierFromSave : MonoBehaviour
{
    [Header("Data Source")]
    [Tooltip("Assign your 'MainCharacterDataAsset' (DataCharacters type) asset here.")]
    public DataCharacters characterDB; // Use OUR database type

    [Header("Target Components on This GameObject")]
    [Tooltip("Assign the SpriteRenderer component FROM THIS GameObject.")]
    public SpriteRenderer targetSpriteRenderer;
    [Tooltip("Assign the Animator component FROM THIS GameObject.")]
    public Animator targetAnimator;

    // Keep track of the skin name currently applied BY THIS SCRIPT
    private string currentAppliedSkinName = "";
    private bool isInitialized = false; // Flag to prevent running apply logic too early

    // --- Use Awake to ensure references are checked early ---
    void Awake()
    {
        // Ensure references are set, disable self if critical ones are missing
        if (characterDB == null) {
            Debug.LogError($"PlayerSkinApplierFromSave ({gameObject.name}): DataCharacters (characterDB) not assigned! Disabling script.");
            this.enabled = false; return;
        }
        if (targetSpriteRenderer == null) {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
            if (targetSpriteRenderer == null) {
                Debug.LogError($"PlayerSkinApplierFromSave ({gameObject.name}): Could not find SpriteRenderer component! Disabling script.");
                this.enabled = false; return;
            }
        }
        if (targetAnimator == null) {
            targetAnimator = GetComponent<Animator>();
            if (targetAnimator == null) {
                Debug.LogError($"PlayerSkinApplierFromSave ({gameObject.name}): Could not find Animator component! Disabling script.");
                this.enabled = false; return; // Animator is likely essential
            }
        }
        Debug.Log($"PlayerSkinApplierFromSave ({gameObject.name}): Awake initialization successful.");
        isInitialized = true; // Mark as ready
    }

    // --- Subscribe/Unsubscribe to Scene Loaded Events ---
    void OnEnable()
    {
        // Only subscribe if initialization in Awake was successful
        if(isInitialized) {
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log($"PlayerSkinApplierFromSave ({gameObject.name}) subscribed to sceneLoaded.");
            // Apply skin immediately when enabled if needed (e.g., starting in a gameplay scene)
            // Use a coroutine to delay slightly
            StartCoroutine(ApplySkinOnEnableIfApplicable());
        }
    }

    void OnDisable()
    {
        // Always try to unsubscribe
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log($"PlayerSkinApplierFromSave ({gameObject.name}) unsubscribed from sceneLoaded.");
    }

    void OnDestroy() { // Added for extra safety
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Coroutine to handle applying skin on Enable to avoid potential timing issues
    IEnumerator ApplySkinOnEnableIfApplicable()
    {
        // Wait one frame to allow other initializations potentially
        yield return null;

        // Check if this script is still enabled and the current scene is NOT excluded
         if (!this.enabled) yield break; // Exit if script got disabled

        Scene currentScene = SceneManager.GetActiveScene();
        bool isExcludedScene = IsSceneExcluded(currentScene.name);

        if (!isExcludedScene)
        {
            Debug.Log($"PlayerSkinApplierFromSave applying skin on Enable for scene: {currentScene.name}");
            LoadAndApplySkin();
        }
    }

    // --- Helper Function to Check Excluded Scenes ---
    bool IsSceneExcluded(string sceneName)
    {
        // !!!!! ----- EDIT THIS LIST ----- !!!!!
        // Add the EXACT names of scenes where the skin should NOT be automatically applied.
        // Examples: Main Menu, Character Selection, Cutscenes, etc.
        // Also include TutorialScene based on Player.cs logic destroying the persistent player there.
        return sceneName == "CharacterSelection" ||
               sceneName == "MainMenu" ||           // <-- Change or remove if needed
               sceneName == "Homescreen" ||        // <-- Change or remove if needed
               sceneName == "TutorialScene";       // <-- Keep this if Player.cs destroys the instance here
               // Add more || sceneName == "OtherExcludedScene" if necessary
    }

    // --- Apply skin whenever ANY relevant scene loads ---
    // <<< THIS IS THE MODIFIED SECTION >>>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Apply the skin if the script is enabled AND the loaded scene is NOT excluded
        if (this.enabled && !IsSceneExcluded(scene.name))
        {
            Debug.Log($"PlayerSkinApplierFromSave applying skin for newly loaded scene: {scene.name}");
            LoadAndApplySkin(); // <<< Apply skin in MOST scenes now
        }
        else if (this.enabled) // Log if it's excluded or script disabled
        {
            Debug.Log($"PlayerSkinApplierFromSave: Scene '{scene.name}' loaded. Skin application skipped (Excluded: {IsSceneExcluded(scene.name)}, Enabled: {this.enabled}).");
        }
         else {
             Debug.LogWarning($"PlayerSkinApplierFromSave: OnSceneLoaded called but script is disabled for {gameObject.name}");
         }
    }


    // Reads PlayerPrefs and applies the skin visuals using OUR database
    void LoadAndApplySkin()
    {
        if (!isInitialized || !this.enabled) { // Added check for enabled status
             Debug.LogWarning($"PlayerSkinApplierFromSave ({gameObject.name}): Attempted to load skin but script is not initialized or disabled. Skipping.");
             return;
        }

        // Force stop any previous verification coroutine if LoadAndApplySkin is called again quickly
        StopCoroutine("VerifySkinAfterDelay"); // Stop by string name

        string savedSkinName = PlayerPrefs.GetString("LastSelectedSkin", "Default");

        // Optional: Check if already applied to prevent redundant work, though reapplying is safe
        // Removed the verification check here to simplify - reapplying is generally safe and ensures correctness after scene load
        // if (savedSkinName == currentAppliedSkinName && ...) { ... }


        Debug.Log($"PlayerSkinApplierFromSave ({gameObject.name}): Loading saved skin name: '{savedSkinName}'");

        // Find the character data using the NAME in OUR database
        DataCharacterEntry characterData = characterDB.GetCharacterByName(savedSkinName);

        if (characterData != null)
        {
            // Apply Sprite
            if (targetSpriteRenderer != null)
            {
                targetSpriteRenderer.sprite = characterData.characterDisplaySprite;
                Debug.Log($"PlayerSkinApplierFromSave ({gameObject.name}): Sprite set to: {characterData.characterDisplaySprite?.name}");
            } else { Debug.LogError($"PlayerSkinApplierFromSave ({gameObject.name}): Target Sprite Renderer is NULL!"); return; } // Stop if component missing


            // Apply Animator Controller
            if (targetAnimator != null)
            {
                 if(characterData.animatorController != null)
                 {
                     // Check if it's already the correct controller to avoid unnecessary Animator resets
                     if(targetAnimator.runtimeAnimatorController != characterData.animatorController)
                     {
                         targetAnimator.runtimeAnimatorController = characterData.animatorController;
                         Debug.Log($"PlayerSkinApplierFromSave ({gameObject.name}): Animator controller set to: {characterData.animatorController.name}");
                     } else {
                          Debug.Log($"PlayerSkinApplierFromSave ({gameObject.name}): Animator controller already set to: {characterData.animatorController.name}. Skipping assignment.");
                     }
                 }
                 else { Debug.LogWarning($"PlayerSkinApplierFromSave ({gameObject.name}): AnimatorController not set for skin '{savedSkinName}' in database."); }
            } else { Debug.LogError($"PlayerSkinApplierFromSave ({gameObject.name}): Target Animator is NULL!"); return; } // Stop if component missing

            // Update the tracker
            currentAppliedSkinName = savedSkinName;

            // *** START THE VERIFICATION COROUTINE (Optional but good for debugging) ***
            StartCoroutine(VerifySkinAfterDelay(characterData.characterDisplaySprite, characterData.animatorController));

        }
        else
        {
            Debug.LogError($"PlayerSkinApplierFromSave ({gameObject.name}): Character data for skin name '{savedSkinName}' not found! Applying Default.");
            ApplyDefaultSkin(); // Apply Default visuals as a fallback
        }
    }

    // Helper to specifically apply the default skin visuals
    void ApplyDefaultSkin() {
         if (!this.enabled || !isInitialized) return; // Prevent running if disabled

         DataCharacterEntry defaultData = characterDB.GetCharacterByName("Default");
            if (defaultData != null) {
                 if(targetSpriteRenderer != null) targetSpriteRenderer.sprite = defaultData.characterDisplaySprite;
                 if (targetAnimator != null && defaultData.animatorController != null) {
                      if(targetAnimator.runtimeAnimatorController != defaultData.animatorController) {
                           targetAnimator.runtimeAnimatorController = defaultData.animatorController;
                           Debug.Log($"PlayerSkinApplierFromSave ({gameObject.name}): Applied Default Animator Controller.");
                      }
                 }

                 currentAppliedSkinName = "Default"; // Track that default was applied

                 // Start verification for default skin too
                 if(targetSpriteRenderer != null && targetAnimator != null) { // Only verify if components exist
                    StartCoroutine(VerifySkinAfterDelay(defaultData.characterDisplaySprite, defaultData.animatorController));
                 }
            } else { Debug.LogError($"PlayerSkinApplierFromSave ({gameObject.name}): Could not find 'Default' skin data in the database!"); }
    }

    // --- Coroutine for Delayed Verification ---
    IEnumerator VerifySkinAfterDelay(Sprite expectedSprite, RuntimeAnimatorController expectedController)
    {
        // Wait until the end of the current frame.
        yield return new WaitForEndOfFrame();

         // Check if the script or components became invalid during the frame
        if (!this.enabled || targetSpriteRenderer == null || targetAnimator == null) {
             Debug.LogWarning($"PlayerSkinApplierFromSave ({gameObject.name}): Verification skipped - script/components became invalid after frame start.");
             yield break;
        }

        Debug.LogWarning($"--- Verification Check START (End Of Frame for '{currentAppliedSkinName}' in scene '{SceneManager.GetActiveScene().name}') ---");

        bool spriteOK = true;
        bool animatorOK = true;

        // Verify Sprite
        if (targetSpriteRenderer.sprite == expectedSprite) {
            // Debug.Log($"Verification: SpriteRenderer.sprite is CORRECT ({targetSpriteRenderer.sprite?.name})");
        } else {
            Debug.LogError($"Verification ({gameObject.name}): SpriteRenderer.sprite has CHANGED! Is now: {targetSpriteRenderer.sprite?.name}, Expected: {expectedSprite?.name}");
            spriteOK = false;
        }

        // Verify Animator Controller
        if (expectedController != null && targetAnimator.runtimeAnimatorController == expectedController) {
           // Debug.Log($"Verification: Animator.runtimeAnimatorController is CORRECT ({targetAnimator.runtimeAnimatorController?.name})");
        }
        else if (targetAnimator.runtimeAnimatorController != expectedController) { // Check cases where it changed, or where one is null and the other isn't
            Debug.LogError($"Verification ({gameObject.name}): Animator.runtimeAnimatorController has CHANGED! Is now: {targetAnimator.runtimeAnimatorController?.name}, Expected: {expectedController?.name}");
             animatorOK = false;
        }


        if(spriteOK && animatorOK) {
             Debug.Log($"Verification ({gameObject.name}): Sprite and Animator Controller are BOTH CORRECT at EndOfFrame.");
        } else {
             Debug.LogError($"Verification ({gameObject.name}): PROBLEM DETECTED at EndOfFrame. Check logs above.");
        }

        Debug.LogWarning($"--- Verification Check END ---");
    }
}