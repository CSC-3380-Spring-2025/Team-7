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

    public string GetCurrentSkinName()
    {
        // No changes needed here
        return string.IsNullOrEmpty(currentAppliedSkinName) ? "Default" : currentAppliedSkinName;
    }

    public void SetCurrentSkin(string skinName)
    {
        Debug.Log($"[PlayerSkinApplier] SetCurrentSkin called with skinName: '{skinName}'"); // Log entry point

        if (!isInitialized || !this.enabled)
        {
            Debug.LogWarning($"[PlayerSkinApplier] SetCurrentSkin skipped: Initialized={isInitialized}, Enabled={this.enabled}");
            return;
        }

        DataCharacterEntry characterData = characterDB.GetCharacterByName(skinName);
        if (characterData != null)
        {
            Debug.Log($"[PlayerSkinApplier] Found Character Data for '{skinName}'. Applying...");

            if (targetSpriteRenderer != null)
            {
                targetSpriteRenderer.sprite = characterData.characterDisplaySprite;
                 Debug.Log($"[PlayerSkinApplier] Applied sprite '{characterData.characterDisplaySprite?.name}' to {targetSpriteRenderer.gameObject.name}");
            }
            else {
                Debug.LogError("[PlayerSkinApplier] Target Sprite Renderer is null during SetCurrentSkin!");
                return;
            }

            if (targetAnimator != null)
            {
                if(characterData.animatorController != null)
                {
                    if(targetAnimator.runtimeAnimatorController != characterData.animatorController)
                    {
                        targetAnimator.runtimeAnimatorController = characterData.animatorController;
                        Debug.Log($"[PlayerSkinApplier] Applied Animator Controller '{characterData.animatorController.name}' to {targetAnimator.gameObject.name}");
                    }
                    else
                    {
                        Debug.Log($"[PlayerSkinApplier] Animator Controller '{characterData.animatorController.name}' already applied.");
                    }
                }
                else
                {
                     Debug.LogWarning($"[PlayerSkinApplier] Character Data for '{skinName}' has no Animator Controller assigned.");
                }
            }
            else {
                 Debug.LogError("[PlayerSkinApplier] Target Animator is null during SetCurrentSkin!");
                 return;
            }

            currentAppliedSkinName = skinName;
            PlayerPrefs.SetString("LastSelectedSkin", skinName); // Keep PlayerPrefs logic if needed, but load overrides this
             Debug.Log($"[PlayerSkinApplier] Successfully applied skin '{skinName}'. Updated PlayerPrefs.");
        }
        else
        {
            Debug.LogWarning($"[PlayerSkinApplier] Character Data not found for '{skinName}'. Attempting to apply default skin.");
            ApplyDefaultSkin();
        }
    }

    void Awake()
    {
        // No logging changes needed here unless troubleshooting initialization
        if (characterDB == null) {
             Debug.LogError("[PlayerSkinApplier] CharacterDB not assigned in Inspector!", this.gameObject);
            this.enabled = false; return;
        }
        if (targetSpriteRenderer == null) {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
            if (targetSpriteRenderer == null) {
                Debug.LogError("[PlayerSkinApplier] Target Sprite Renderer not found on GameObject or assigned!", this.gameObject);
                this.enabled = false; return;
            }
        }
        if (targetAnimator == null) {
            targetAnimator = GetComponent<Animator>();
            if (targetAnimator == null) {
                 Debug.LogError("[PlayerSkinApplier] Target Animator not found on GameObject or assigned!", this.gameObject);
                this.enabled = false; return;
            }
        }
        isInitialized = true;
         Debug.Log($"[PlayerSkinApplier] Initialized on {this.gameObject.name}");
    }

    void OnEnable()
    {
        // No logging changes needed here unless troubleshooting timing
        if(isInitialized) {
            SceneManager.sceneLoaded += OnSceneLoaded;
            StartCoroutine(ApplySkinOnEnableIfApplicable());
        }
    }

    void OnDisable()
    {
        // No changes needed here
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnDestroy() {
        // No changes needed here
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    IEnumerator ApplySkinOnEnableIfApplicable()
    {
        // No logging changes needed here unless troubleshooting timing
        yield return null;

        if (!this.enabled) yield break;

        Scene currentScene = SceneManager.GetActiveScene();
        if (!IsSceneExcluded(currentScene.name))
        {
            LoadAndApplySkin();
        }
    }

    bool IsSceneExcluded(string sceneName)
    {
        // No changes needed here
        return sceneName == "CharacterSelection" ||
               sceneName == "Homescreen";
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
         Debug.Log($"[PlayerSkinApplier] OnSceneLoaded: {scene.name} (Mode: {mode})");
        if (this.enabled && !IsSceneExcluded(scene.name))
        {
             Debug.Log($"[PlayerSkinApplier] Scene '{scene.name}' is not excluded, calling LoadAndApplySkin().");
            LoadAndApplySkin(); // This might be overwriting the skin set by MongoDBSaveLoadManager if load happens after this
        }
         else
        {
            Debug.Log($"[PlayerSkinApplier] Scene '{scene.name}' is excluded or component disabled. Not applying skin via OnSceneLoaded.");
        }
    }

    void LoadAndApplySkin()
    {
        // This method uses PlayerPrefs, which might conflict with the load from the database.
        // Adding logs to see when it runs.
        Debug.Log($"[PlayerSkinApplier] LoadAndApplySkin called (uses PlayerPrefs). Initialized={isInitialized}, Enabled={this.enabled}");

        if (!isInitialized || !this.enabled) {
             Debug.LogWarning($"[PlayerSkinApplier] LoadAndApplySkin skipped: Initialized={isInitialized}, Enabled={this.enabled}");
             return;
        }

        string savedSkinName = PlayerPrefs.GetString("LastSelectedSkin", "Default");
        Debug.Log($"[PlayerSkinApplier] Found skin '{savedSkinName}' in PlayerPrefs. Calling SetCurrentSkin with this value.");
        SetCurrentSkin(savedSkinName);
    }

    void ApplyDefaultSkin() {
         Debug.Log("[PlayerSkinApplier] ApplyDefaultSkin called.");
         if (!this.enabled || !isInitialized)
         {
             Debug.LogWarning($"[PlayerSkinApplier] ApplyDefaultSkin skipped: Initialized={isInitialized}, Enabled={this.enabled}");
             return;
         }

         DataCharacterEntry defaultData = characterDB.GetCharacterByName("Default");
            if (defaultData != null) {
                 Debug.Log("[PlayerSkinApplier] Found default character data."); // Old Log 6
                 if(targetSpriteRenderer != null)
                 {
                     targetSpriteRenderer.sprite = defaultData.characterDisplaySprite;
                     Debug.Log($"[PlayerSkinApplier] Applied default sprite '{defaultData.characterDisplaySprite?.name}'");
                 }


                 if (targetAnimator != null && defaultData.animatorController != null) {
                    Debug.Log("[PlayerSkinApplier] Applying default animator controller..."); // Old Log 7
                      if(targetAnimator.runtimeAnimatorController != defaultData.animatorController) {
                           Debug.Log("[PlayerSkinApplier] Default animator controller is different, setting it."); // Old Log 8
                           targetAnimator.runtimeAnimatorController = defaultData.animatorController;
                      }
                      else
                      {
                          Debug.Log("[PlayerSkinApplier] Default animator controller already applied.");
                      }
                 }
                 currentAppliedSkinName = "Default";
                 PlayerPrefs.SetString("LastSelectedSkin", "Default");
                 Debug.Log("[PlayerSkinApplier] Finished applying default skin. Set PlayerPrefs to Default.");
            }
            else
            {
                Debug.LogError("[PlayerSkinApplier] CRITICAL: Could not find 'Default' character data in CharacterDB!");
            }
    }
}