using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GachaMachine : MonoBehaviour
{
    public static GachaMachine Instance { get; private set; }

    [Header("Skin Sprites")]
    public Sprite defaultSprite;
    public Sprite WhiteCat;
    public Sprite BlackCat;
    public Sprite ShortCat;
    public Sprite SiameseCat;
    public Sprite CalicoCat;

    [Header("Skin Names By Rarity")]
    [SerializeField] private List<string> rareSkins = new List<string> { "WhiteCat", "BlackCat" };
    [SerializeField] private List<string> superRareSkins = new List<string> { "ShortCat", "SiameseCat" };
    [SerializeField] private List<string> ultraSkins = new List<string> { "CalicoCat" };

    public HashSet<string> mySkins = new HashSet<string>() { "Default" };
    public Dictionary<string, Sprite> skinSprites = new Dictionary<string, Sprite>();

    private GameObject pawsVisualObject;
    private GameObject coinsVisualObject;
    private GameObject snakeVisualObject;
    private List<GameObject> noMatchVisualVariants = new List<GameObject>();
    private TextMeshProUGUI resultText;
    private Image skinDisplay;

    [Header("Result Display Settings")]
    [SerializeField] private float winDisplayDelay = 1.5f;

    [Header("Scene Configuration")]
    [Tooltip("The exact name of the scene file containing the Gacha UI and visuals.")]
    [SerializeField] private string gachaSceneName = "GachaScene";

    [Tooltip("The name of the GameObject that is the parent of the Result Text and Skin Display (likely the Canvas).")]
    [SerializeField] private string uiParentName = "LeverScreen";

    [Tooltip("The name of the GameObject that is the parent of Paws, Coins, Snake, and NoMatch visuals.")]
    [SerializeField] private string visualParentName = "LeverScreen";

    [Tooltip("The exact name of the child GameObject holding the Result Text component.")]
    [SerializeField] private string resultTextObjectName = "ResultText";

    [Tooltip("The exact name of the child GameObject holding the Skin Display Image component.")]
    [SerializeField] private string skinDisplayObjectName = "SkinDisplay";

    [Header("No Match Variant Setup")]
    [Tooltip("The Tag assigned to all 'No Match' visual variant GameObjects.")]
    [SerializeField] private string noMatchVisualTag = "NoMatchVisual";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayerPrefs.DeleteKey("LastSelectedSkin");
        PlayerPrefs.Save();

        InitializeSkinDictionary();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gachaSceneName)
        {
            FindSceneReferences();
            ResetGachaState();
        }
        else
        {
            ClearSceneReferences();
        }
    }

    void FindSceneReferences()
    {
        bool allFound = true;

        GameObject uiParentObject = GameObject.Find(uiParentName);
        if (uiParentObject != null)
        {
            Transform uiParentTransform = uiParentObject.transform;

            Transform resultTextTransform = uiParentTransform.Find(resultTextObjectName);
            if (resultTextTransform != null)
            {
                resultText = resultTextTransform.GetComponent<TextMeshProUGUI>();
                if (resultText == null) { allFound = false; }
            }
            else { resultText = null; allFound = false; }

            Transform skinDisplayTransform = uiParentTransform.Find(skinDisplayObjectName);
            if (skinDisplayTransform != null)
            {
                skinDisplay = skinDisplayTransform.GetComponent<Image>();
                if (skinDisplay == null) { allFound = false; }
            }
            else { skinDisplay = null; allFound = false; }
        }
        else
        {
            resultText = null;
            skinDisplay = null;
            allFound = false;
        }

        GameObject visualParentObject = GameObject.Find(visualParentName);
        if (visualParentObject != null)
        {
            Transform visualParentTransform = visualParentObject.transform;

            pawsVisualObject = visualParentTransform.Find("Paws")?.gameObject;
            coinsVisualObject = visualParentTransform.Find("Coins")?.gameObject;
            snakeVisualObject = visualParentTransform.Find("Snake")?.gameObject;

            if (pawsVisualObject == null) { allFound = false; }
            if (coinsVisualObject == null) { allFound = false; }
            if (snakeVisualObject == null) { allFound = false; }

            noMatchVisualVariants.Clear();
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(noMatchVisualTag);
            foreach (GameObject taggedObj in taggedObjects)
            {
                if (taggedObj != null && taggedObj.transform.IsChildOf(visualParentTransform))
                {
                    noMatchVisualVariants.Add(taggedObj);
                }
            }
            if (noMatchVisualVariants.Count == 0) { allFound = false; }
        }
        else
        {
            pawsVisualObject = null;
            coinsVisualObject = null;
            snakeVisualObject = null;
            noMatchVisualVariants.Clear();
            allFound = false;
        }
    }

    void ClearSceneReferences()
    {
        pawsVisualObject = null;
        coinsVisualObject = null;
        snakeVisualObject = null;
        noMatchVisualVariants.Clear();
        resultText = null;
        skinDisplay = null;
    }

    private void ResetGachaState()
    {
        HideAllVisuals();
        if (resultText != null) resultText.text = ""; 
        if (skinDisplay != null) skinDisplay.gameObject.SetActive(false); 
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
    }

    private void HideAllVisuals()
    {
        pawsVisualObject?.SetActive(false);
        coinsVisualObject?.SetActive(false);
        snakeVisualObject?.SetActive(false);
        if (noMatchVisualVariants != null)
        {
            foreach (GameObject variant in noMatchVisualVariants)
            {
                variant?.SetActive(false);
            }
        }
    }

    public void StartGachaSequence()
    {
        if (resultText == null || skinDisplay == null || pawsVisualObject == null || coinsVisualObject == null || snakeVisualObject == null || noMatchVisualVariants == null)
        {
            if (resultText != null) resultText.text = "Gacha Error!";
            return;
        }
        if (noMatchVisualVariants.Count == 0) 

        StopAllCoroutines();
        StartCoroutine(GachaRollCoroutine());
    }

    private IEnumerator GachaRollCoroutine()
    {
        HideAllVisuals();
        if (resultText != null) resultText.text = "";
        if (skinDisplay != null) skinDisplay.gameObject.SetActive(false);

        int roll = UnityEngine.Random.Range(1, 3001);
        GameObject visualToActivate = null;
        bool wonNewSkin = false;
        string skinWonName = null;

        if (roll <= 100 && ultraSkins.Count > 0)
        {
            string selectedSkin = ultraSkins[UnityEngine.Random.Range(0, ultraSkins.Count)];
            if (mySkins.Add(selectedSkin))
            {
                visualToActivate = snakeVisualObject;
                wonNewSkin = true;
                skinWonName = selectedSkin;
            }
        }
        else if (roll <= 220 && superRareSkins.Count > 0)
        {
            string selectedSkin = superRareSkins[UnityEngine.Random.Range(0, superRareSkins.Count)];
            if (mySkins.Add(selectedSkin))
            {
                visualToActivate = coinsVisualObject;
                wonNewSkin = true;
                skinWonName = selectedSkin;
            }
        }
        else if (roll <= 520 && rareSkins.Count > 0)
        {
            string selectedSkin = rareSkins[UnityEngine.Random.Range(0, rareSkins.Count)];
            if (mySkins.Add(selectedSkin))
            {
                visualToActivate = pawsVisualObject;
                wonNewSkin = true;
                skinWonName = selectedSkin;
            }
        }

        if (!wonNewSkin)
        {
            if (noMatchVisualVariants != null && noMatchVisualVariants.Count > 0)
            {
                visualToActivate = noMatchVisualVariants[Random.Range(0, noMatchVisualVariants.Count)];
            }
        }

        if (visualToActivate != null)
        {
            visualToActivate.SetActive(true);
        }
    

        yield return new WaitForSeconds(winDisplayDelay);

        if (resultText != null)
        {
            resultText.text = (wonNewSkin && skinWonName != null) ? $"You won {skinWonName}!" : "Try Again!";
        }

        if (wonNewSkin && skinWonName != null)
        {
            if (skinDisplay != null)
            {
                if (skinSprites.TryGetValue(skinWonName, out Sprite spriteToShow))
                {
                    skinDisplay.sprite = spriteToShow;
                    skinDisplay.gameObject.SetActive(true);
                }
                else
                {
                    skinDisplay.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (skinDisplay != null) skinDisplay.gameObject.SetActive(false);
        }
    }
}