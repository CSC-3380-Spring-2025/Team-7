using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Applies saved skin preferences to the player character visuals.
public class PlayerSkinApplierFromSave : MonoBehaviour
{
    // Reference to the database containing character skin data.
    public DataCharacters characterDB;
    // The SpriteRenderer component on the player character to modify.
    public SpriteRenderer targetSpriteRenderer;
    // The Animator component on the player character to modify.
    public Animator targetAnimator;

    private string currentAppliedSkinName = "";
    private bool isInitialized = false;

    // Gets the name of the currently applied skin.
    public string GetCurrentSkinName()
    {
        // Return "Default" if no skin name is set yet.
        return string.IsNullOrEmpty(currentAppliedSkinName) ? "Default" : currentAppliedSkinName;
    }

    // Finds skin data by name, applies visuals (sprite/animator), and saves the choice.
    public void SetCurrentSkin(string skinName)
    {
        if (!isInitialized || !this.enabled) { return; }

        DataCharacterEntry characterData = characterDB.GetCharacterByName(skinName);
        if (characterData != null)
        {
            if (targetSpriteRenderer != null)
            { targetSpriteRenderer.sprite = characterData.characterDisplaySprite; }
            else { return; } // Stop if renderer is missing

            if (targetAnimator != null && characterData.animatorController != null)
            {
                // Only assign if different to avoid unnecessary animator resets.
                if(targetAnimator.runtimeAnimatorController != characterData.animatorController)
                { targetAnimator.runtimeAnimatorController = characterData.animatorController; }
            }
            else if (targetAnimator == null) { return; } // Stop if animator is missing

            // Update internal state and save the choice persistently.
            currentAppliedSkinName = skinName;
            PlayerPrefs.SetString("LastSelectedSkin", skinName);
            PlayerPrefs.Save();
        }
        else
        {
            // If the requested skin isn't found, revert to default.
            ApplyDefaultSkin();
        }
    }

    // Initializes references and ensures the script is ready.
    void Awake()
    {
        if (characterDB == null) { this.enabled = false; return; }

        if (targetSpriteRenderer == null) {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
            if (targetSpriteRenderer == null) { this.enabled = false; return; }
        }
        if (targetAnimator == null) {
            targetAnimator = GetComponent<Animator>();
            if (targetAnimator == null) { this.enabled = false; return; }
        }

        // --- REMOVED THE PlayerPrefs.DeleteKey CALL ---
        // PlayerPrefs.DeleteKey("LastSelectedSkin");
        // PlayerPrefs.Save();
        // --- END REMOVAL ---

        isInitialized = true;
    }

    // Subscribes to scene load events and triggers initial skin application.
    void OnEnable()
    {
        if(isInitialized) {
            SceneManager.sceneLoaded += OnSceneLoaded;
            // Use coroutine to ensure loading happens after potential scene setup.
            StartCoroutine(ApplySkinOnEnableIfApplicable());
        }
    }

    // Unsubscribes from scene load events.
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Ensures unsubscription from scene load events on destruction.
    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Coroutine to wait briefly then load/apply skin if the scene is not excluded.
    IEnumerator ApplySkinOnEnableIfApplicable()
    {
        yield return null; // Wait one frame
        if (!this.enabled || !isInitialized) yield break;

        Scene currentScene = SceneManager.GetActiveScene();
        // Apply skin if the current scene is NOT one where selection happens.
        if (!IsSceneExcluded(currentScene.name))
        {
            LoadAndApplySkin();
        }
    }

    // Checks if the given scene name should skip automatic skin loading.
    bool IsSceneExcluded(string sceneName)
    {
        // Define scenes where the player makes choices, not where skin should be auto-applied.
        return sceneName == "CharacterSelection" || sceneName == "Homescreen";
    }

    // Event handler for scene loads; reapplies skin if necessary.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this.enabled && isInitialized && !IsSceneExcluded(scene.name))
        {
            // Re-apply skin when entering a valid gameplay scene.
            LoadAndApplySkin();
        }
    }

    // Loads the saved skin name from PlayerPrefs and applies it.
    void LoadAndApplySkin()
    {
        if (!isInitialized || !this.enabled) { return; }
        // Load the last saved choice, defaulting to "Default".
        string savedSkinName = PlayerPrefs.GetString("LastSelectedSkin", "Default");
        SetCurrentSkin(savedSkinName);
    }

    // Applies the specific 'Default' skin visuals and saves the preference.
    void ApplyDefaultSkin() {
         if (!this.enabled || !isInitialized) { return; }
         DataCharacterEntry defaultData = characterDB.GetCharacterByName("Default");
         if (defaultData != null) {
             if(targetSpriteRenderer != null) { targetSpriteRenderer.sprite = defaultData.characterDisplaySprite; }
             if (targetAnimator != null && defaultData.animatorController != null) {
                 if(targetAnimator.runtimeAnimatorController != defaultData.animatorController)
                 { targetAnimator.runtimeAnimatorController = defaultData.animatorController; }
             }
             // Apply and save "Default" as the current skin.
             currentAppliedSkinName = "Default";
             PlayerPrefs.SetString("LastSelectedSkin", "Default");
             PlayerPrefs.Save();
         }
    }
}