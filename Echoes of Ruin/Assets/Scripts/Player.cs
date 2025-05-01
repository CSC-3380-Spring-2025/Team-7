using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public DataCharacters characterDB;
    public SpriteRenderer ArtworkSprite;
    public Animator Animator;

    private static Vector3 savedPosition;
    private static bool positionWasSaved = false;
    private static string scenePlayerWasLastIn = "";

    private static Player instance;

    void Awake()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (instance == this && positionWasSaved && currentSceneName == "ForestClearing")
        {
            transform.position = new Vector3(savedPosition.x, savedPosition.y, transform.position.z);
        }

        string savedSkinName = PlayerPrefs.GetString("LastSelectedSkin", "Default");
        UpdateCharacterByName(savedSkinName);

        SetupCameraFollow();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        string currentSceneName = SceneManager.GetActiveScene().name;
         if (instance == this && gameObject.scene.isLoaded && currentSceneName == "ForestClearing")
         {
             savedPosition = transform.position;
             positionWasSaved = true;
             scenePlayerWasLastIn = currentSceneName;
         }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
         if (scene.name == "ForestClearing" && instance == this)
         {
             string skinToLoad = PlayerPrefs.GetString("LastSelectedSkin", "Default");
             UpdateCharacterByName(skinToLoad);
             SetupCameraFollow();
         }
    }

    private void UpdateCharacterByName(string skinName)
    {
        if (characterDB == null) { return; }

        DataCharacterEntry characterData = characterDB.GetCharacterByName(skinName);

        if (characterData != null)
        {
            if (ArtworkSprite != null)
            {
                ArtworkSprite.sprite = characterData.characterDisplaySprite;
            }
            if (Animator != null)
            {
                 if (characterData.animatorController != null)
                 {
                     Animator.runtimeAnimatorController = characterData.animatorController;
                 }
            }
        }
        else
        {
            DataCharacterEntry defaultData = characterDB.GetCharacterByName("Default");
            if (defaultData != null)
            {
                 if (ArtworkSprite != null)
                 {
                     ArtworkSprite.sprite = defaultData.characterDisplaySprite;
                 }
                 if (Animator != null && defaultData.animatorController != null)
                 {
                     Animator.runtimeAnimatorController = defaultData.animatorController;
                 }
            }
        }
    }

    void SetupCameraFollow()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            CameraFollow follow = cam.GetComponent<CameraFollow>();
            if (follow != null)
            {
                if (instance == this)
                {
                    follow.Target = this.transform;
                }
            }
        }
    }
}