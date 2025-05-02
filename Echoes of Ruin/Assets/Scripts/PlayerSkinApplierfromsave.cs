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

    void Awake()
    {
        if (characterDB == null) {
            this.enabled = false; return;
        }
        if (targetSpriteRenderer == null) {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
            if (targetSpriteRenderer == null) {
                this.enabled = false; return;
            }
        }
        if (targetAnimator == null) {
            targetAnimator = GetComponent<Animator>();
            if (targetAnimator == null) {
                this.enabled = false; return;
            }
        }
        isInitialized = true;
    }

    void OnEnable()
    {
        if(isInitialized) {
            SceneManager.sceneLoaded += OnSceneLoaded;
            StartCoroutine(ApplySkinOnEnableIfApplicable());
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    IEnumerator ApplySkinOnEnableIfApplicable()
    {
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
        return sceneName == "CharacterSelection" ||
               sceneName == "Homescreen" ||
               sceneName == "TutorialScene";
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this.enabled && !IsSceneExcluded(scene.name))
        {
            LoadAndApplySkin();
        }
    }

    void LoadAndApplySkin()
    {
        if (!isInitialized || !this.enabled) {
             return;
        }

        string savedSkinName = PlayerPrefs.GetString("LastSelectedSkin", "Default");

        DataCharacterEntry characterData = characterDB.GetCharacterByName(savedSkinName);

        if (characterData != null)
        {
            if (targetSpriteRenderer != null)
            {
                targetSpriteRenderer.sprite = characterData.characterDisplaySprite;
            } else { return; }

            if (targetAnimator != null)
            {
                 if(characterData.animatorController != null)
                 {
                     if(targetAnimator.runtimeAnimatorController != characterData.animatorController)
                     {
                         targetAnimator.runtimeAnimatorController = characterData.animatorController;
                     }
                 }
            } else { return; }

            currentAppliedSkinName = savedSkinName;
        }
        else
        {
            ApplyDefaultSkin();
        }
    }

    void ApplyDefaultSkin() {
         if (!this.enabled || !isInitialized) return;

         DataCharacterEntry defaultData = characterDB.GetCharacterByName("Default");
            if (defaultData != null) {
                 if(targetSpriteRenderer != null) targetSpriteRenderer.sprite = defaultData.characterDisplaySprite;
                 if (targetAnimator != null && defaultData.animatorController != null) {
                      if(targetAnimator.runtimeAnimatorController != defaultData.animatorController) {
                           targetAnimator.runtimeAnimatorController = defaultData.animatorController;
                      }
                 }
                 currentAppliedSkinName = "Default";
            }
    }
}