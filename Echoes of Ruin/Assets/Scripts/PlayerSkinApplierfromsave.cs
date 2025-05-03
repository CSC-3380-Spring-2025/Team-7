using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSkinApplierFromSave : MonoBehaviour
{
    public DataCharacters characterDB;
    public SpriteRenderer targetSpriteRenderer;
    public Animator targetAnimator;
    private string currentAppliedSkinName = ""; 
    private bool isInitialized = false; 

    // Gets the name of the currently applied skin.
    public string GetCurrentSkinName()
    {
        if (string.IsNullOrEmpty(currentAppliedSkinName))
        {
            return "Default";
        }
        else
        {
            return currentAppliedSkinName;
        }
    }

    // Finds skin data by name, applies visuals (sprite/animator), and saves the choice.
    public void SetCurrentSkin(string skinName)
    {
        if (!isInitialized || !this.enabled) { return; } 

        DataCharacterEntry characterData = characterDB.GetCharacterByName(skinName);
        if (characterData != null) // Found the requested skin data
        {
            if (targetSpriteRenderer != null)
            {
                targetSpriteRenderer.sprite = characterData.characterDisplaySprite;
            }
            else
            {
                return; 
            }

            if (targetAnimator != null)
            {
                if(characterData.animatorController != null)
                {
                    if(targetAnimator.runtimeAnimatorController != characterData.animatorController)
                    {
                        targetAnimator.runtimeAnimatorController = characterData.animatorController;
                    }
                }
            }
            else
            {  
                return; 
            }

            currentAppliedSkinName = skinName;
            PlayerPrefs.SetString("LastSelectedSkin", skinName); 
            PlayerPrefs.Save();
        }
        else 
        {
            ApplyDefaultSkin(); 
        }
    }

    // Initializes references, clears saved skin preference, and sets initialized flag.
    void Awake()
    {
        if (characterDB == null) {
             Debug.LogError("[PlayerSkinApplier] CharacterDB not assigned in Inspector Disabling script.", this.gameObject); 
            this.enabled = false; return;
        }

        if (targetSpriteRenderer == null) {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
            if (targetSpriteRenderer == null) {
                 Debug.LogError("[PlayerSkinApplier] Target Sprite Renderer not found on GameObject or assigned Disabling script.", this.gameObject); 
                this.enabled = false; return;
            }
        }
        if (targetAnimator == null) {
            targetAnimator = GetComponent<Animator>();
            if (targetAnimator == null) {
                 Debug.LogError("[PlayerSkinApplier] Target Animator not found on GameObject or assigned Disabling script.", this.gameObject); 
                this.enabled = false; return;
            }
        }

        // Clear previously saved skin to force default on game start
        PlayerPrefs.DeleteKey("LastSelectedSkin");
        PlayerPrefs.Save();

        isInitialized = true;
    }

    // Subscribes to scene load events and triggers initial skin application.
    void OnEnable()
    {
        if(isInitialized) {
            SceneManager.sceneLoaded += OnSceneLoaded;
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
        yield return null;

        if (!this.enabled || !isInitialized) yield break;

        Scene currentScene = SceneManager.GetActiveScene();
        if (!IsSceneExcluded(currentScene.name))
        {
            LoadAndApplySkin();
        }
    }

    // Checks if the given scene name should skip automatic skin loading.
    bool IsSceneExcluded(string sceneName)
    {
        return sceneName == "CharacterSelection" ||
               sceneName == "Homescreen";
    }

    // Event handler for scene loads; reapplies skin if necessary.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this.enabled && isInitialized && !IsSceneExcluded(scene.name))
        {
            LoadAndApplySkin();
        }
    }

    // Loads the saved skin name from PlayerPrefs and applies it.
    void LoadAndApplySkin()
    {
        if (!isInitialized || !this.enabled)
        { 
            return; 
        }

        string savedSkinName = PlayerPrefs.GetString("LastSelectedSkin", "Default");
        SetCurrentSkin(savedSkinName); 
    }

    // Applies the specific 'Default' skin visuals and saves the preference.
    void ApplyDefaultSkin() {
         if (!this.enabled || !isInitialized) { return; }

         DataCharacterEntry defaultData = characterDB.GetCharacterByName("Default");
            if (defaultData != null) { 
                 if(targetSpriteRenderer != null) {
                     targetSpriteRenderer.sprite = defaultData.characterDisplaySprite;
                 } else { Debug.LogError("[PlayerSkinApplier] Cannot apply default sprite: targetSpriteRenderer is null!");}

                 if (targetAnimator != null) {
                     if(defaultData.animatorController != null) {
                         if(targetAnimator.runtimeAnimatorController != defaultData.animatorController) {
                               targetAnimator.runtimeAnimatorController = defaultData.animatorController;
                         }
                     }
                 } else { Debug.LogError("[PlayerSkinApplier] Cannot apply default animator: targetAnimator is null!");} 

                 currentAppliedSkinName = "Default";
                 PlayerPrefs.SetString("LastSelectedSkin", "Default");
                 PlayerPrefs.Save();
            }
            else
            {
                Debug.LogError("[PlayerSkinApplier] CRITICAL: Could not find 'Default' character data in CharacterDB! Cannot apply default skin.");
            }
    }
}
