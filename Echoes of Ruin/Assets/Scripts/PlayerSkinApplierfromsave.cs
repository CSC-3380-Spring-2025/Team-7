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
               sceneName == "Homescreen";
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
            Debug.Log("1");
            if (targetSpriteRenderer != null)
            {
                targetSpriteRenderer.sprite = characterData.characterDisplaySprite;
                Debug.Log("2");
            } else { return; }

            if (targetAnimator != null)
            {
                Debug.Log("3");
                 if(characterData.animatorController != null)
                 {
                    Debug.Log("4");
                     if(targetAnimator.runtimeAnimatorController != characterData.animatorController)
                     {
                        Debug.Log("5");
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
                Debug.Log("6");
                 if(targetSpriteRenderer != null) targetSpriteRenderer.sprite = defaultData.characterDisplaySprite;

                 if (targetAnimator != null && defaultData.animatorController != null) {
                    Debug.Log("7");
                      if(targetAnimator.runtimeAnimatorController != defaultData.animatorController) {
                        Debug.Log("8");
                           targetAnimator.runtimeAnimatorController = defaultData.animatorController;
                      }
                 }
                 currentAppliedSkinName = "Default";
            }
    }
}