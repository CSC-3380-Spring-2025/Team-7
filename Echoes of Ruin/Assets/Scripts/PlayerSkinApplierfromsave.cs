using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class PlayerSkinApplierFromSave : MonoBehaviour
{
    public DataCharacters characterDB;
    public SpriteRenderer targetSpriteRenderer;
    public Animator targetAnimator;
    private static string sessionSelectedSkinName = "Default"; 
    private string currentAppliedSkinName = "";
    private bool isInitialized = false;

    // Gets the name of the currently applied skin on this instance.
    public string GetCurrentSkinName()
    {
        return string.IsNullOrEmpty(currentAppliedSkinName) ? "Default" : currentAppliedSkinName;
    }

    // This updates the choice that will be applied when loading gameplay scenes.
    public static void SelectSkinForSession(string skinName)
    {
        if (string.IsNullOrEmpty(skinName)) {
             Debug.LogWarning("[PlayerSkinApplier] Attempted to select a null or empty skin name. Defaulting.");
             sessionSelectedSkinName = "Default";
        } else {
             sessionSelectedSkinName = skinName;
             Debug.Log($"[PlayerSkinApplier] Session skin choice updated to: {sessionSelectedSkinName}");
        }
    }

    // Applies visuals for a specific skin name to this instance. Called internally.
    public void SetCurrentSkin(string skinName)
    {
        if (!isInitialized || !this.enabled) { return; }

        DataCharacterEntry characterData = characterDB.GetCharacterByName(skinName);
        if (characterData != null)
        {
            if (targetSpriteRenderer != null)
            { targetSpriteRenderer.sprite = characterData.characterDisplaySprite; }
            else { return; }

            if (targetAnimator != null && characterData.animatorController != null)
            {
                if(targetAnimator.runtimeAnimatorController != characterData.animatorController)
                { targetAnimator.runtimeAnimatorController = characterData.animatorController; }
            }
            else if (targetAnimator == null) { return; }
            currentAppliedSkinName = skinName;
        }
        else
        {
            ApplyDefaultSkin();
        }
    }

    // Initializes references.
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
        isInitialized = true;
        ApplyDefaultSkin();
    }

    // Subscribes to scene load events.
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

    // Ensures unsubscription.
    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Waits then applies the session's chosen skin if applicable.
    IEnumerator ApplySkinOnEnableIfApplicable()
    {
        yield return null;
        if (!this.enabled || !isInitialized) yield break;
        Scene currentScene = SceneManager.GetActiveScene();
        if (!IsSceneExcluded(currentScene.name))
        {
            LoadAndApplySessionSkin(); 
        }
    }

    // Checks if the scene should skip automatic skin loading.
    bool IsSceneExcluded(string sceneName)
    {
        return sceneName == "CharacterSelection" || sceneName == "Homescreen";
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this.enabled && isInitialized && !IsSceneExcluded(scene.name))
        {
            LoadAndApplySessionSkin(); 
        }
    }

    // Reads the session's chosen skin name and applies it visually.
    void LoadAndApplySessionSkin() 
    {
        if (!isInitialized || !this.enabled) { return; }
        string skinToApply = sessionSelectedSkinName;
        SetCurrentSkin(skinToApply);
    }

    // Applies the 'Default' skin visuals to this instance. Called internally.
    void ApplyDefaultSkin() {
         if (!this.enabled || !isInitialized) { return; }
         DataCharacterEntry defaultData = characterDB.GetCharacterByName("Default");
         if (defaultData != null) {
             if(targetSpriteRenderer != null) { targetSpriteRenderer.sprite = defaultData.characterDisplaySprite; }
             if (targetAnimator != null && defaultData.animatorController != null) {
                 if(targetAnimator.runtimeAnimatorController != defaultData.animatorController)
                 { targetAnimator.runtimeAnimatorController = defaultData.animatorController; }
             }
             currentAppliedSkinName = "Default";
         }
         else { Debug.LogError("[PlayerSkinApplier] CRITICAL: Could not find 'Default' character data!"); }
    }
}