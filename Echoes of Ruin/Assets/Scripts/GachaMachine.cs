using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GachaMachine : MonoBehaviour
{
    public static GachaMachine Instance { get; private set; }

    public Sprite defaultSprite;
    public Sprite WhiteCat;
    public Sprite BlackCat;
    public Sprite ShortCat;
    public Sprite SiameseCat;
    public Sprite CalicoCat;

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
    private GameObject resultTextBackgroundObject;
    private Image skinDisplay;

    private int GACHA_COST = 2;
    [SerializeField] private float winDisplayDelay = 1.0f;
    [SerializeField] private string gachaSceneName = "GachaScene";
    [SerializeField] private string uiParentName = "LeverScreen";
    [SerializeField] private string visualParentName = "LeverScreen";
    [SerializeField] private string resultBackgroundObjectName = "ResultBackground";
    [SerializeField] private string resultTextObjectName = "ResultText";
    [SerializeField] private string skinDisplayObjectName = "SkinDisplay";
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
                }
            }

            Transform skinDisplayTransform = uiParentTransform.Find(skinDisplayObjectName);
            if (skinDisplayTransform != null)
            {
                skinDisplay = skinDisplayTransform.GetComponent<Image>();
            }
        }

        GameObject visualParentObject = GameObject.Find(visualParentName);
        if (visualParentObject != null)
        {
            Transform visualParentTransform = visualParentObject.transform;

            Transform pawsTransform = visualParentTransform.Find("Paws");
            if (pawsTransform != null) pawsVisualObject = pawsTransform.gameObject;

            Transform coinsTransform = visualParentTransform.Find("Coins");
            if (coinsTransform != null) coinsVisualObject = coinsTransform.gameObject;

            Transform snakeTransform = visualParentTransform.Find("Snake");
            if (snakeTransform != null) snakeVisualObject = snakeTransform.gameObject;

            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(noMatchVisualTag);
            foreach (GameObject taggedObj in taggedObjects)
            {
                if (taggedObj != null && taggedObj.transform.IsChildOf(visualParentTransform))
                {
                    noMatchVisualVariants.Add(taggedObj);
                }
            }
        }
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

        if (resultTextBackgroundObject != null)
        {
            resultTextBackgroundObject.SetActive(false);
        }
        if (resultText != null)
        {
            resultText.text = "";
        }

        if (skinDisplay != null)
        {
            skinDisplay.gameObject.SetActive(false);
        }
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
        Currency currencyComponent = null;
        GameObject currencyHolderObject = GameObject.Find("HeartsAndCoinsOverlay");

        if (currencyHolderObject != null)
        {
            currencyComponent = currencyHolderObject.GetComponent<Currency>();
            if (currencyComponent == null)
            {
                if (resultTextBackgroundObject != null) resultTextBackgroundObject.SetActive(true);
                if (resultText != null) resultText.text = "Currency Setup Error!";
                return;
            }
        }
        else
        {
            if (resultTextBackgroundObject != null) resultTextBackgroundObject.SetActive(true);
            if (resultText != null) resultText.text = "Currency System Error!";
            return;
        }

        if (currencyComponent.coin >= GACHA_COST)
        {
            currencyComponent.coin -= GACHA_COST;
            StartCoroutine(GachaRollCoroutine());
        }
        else
        {
            if (resultTextBackgroundObject != null)
            {
                resultTextBackgroundObject.SetActive(true);
            }
            if (resultText != null)
            {
                resultText.text = $"Need {GACHA_COST} Coins!";
            }
            return;
        }
    }


    private IEnumerator GachaRollCoroutine()
    {
        HideAllVisuals();
        if (resultTextBackgroundObject != null) resultTextBackgroundObject.SetActive(false);
        if (skinDisplay != null) skinDisplay.gameObject.SetActive(false);


        int roll = UnityEngine.Random.Range(1, 3001);
        GameObject visualToActivate = null;
        bool wonNewSkin = false;
        string skinWonName = null;

        if (roll <= 100) {
            string selectedSkin = ultraSkins[UnityEngine.Random.Range(0, ultraSkins.Count)];
            if (mySkins.Add(selectedSkin)) 
            { 
            visualToActivate = snakeVisualObject; 
            wonNewSkin = true; 
            skinWonName = selectedSkin; 
            }
        } else if (roll <= 220) {
            string selectedSkin = superRareSkins[UnityEngine.Random.Range(0, superRareSkins.Count)];
            if (mySkins.Add(selectedSkin)) 
            { 
            visualToActivate = coinsVisualObject; 
            wonNewSkin = true; 
            skinWonName = selectedSkin; 
            }
        } else if (roll <= 520) {
            string selectedSkin = rareSkins[UnityEngine.Random.Range(0, rareSkins.Count)];
            if (mySkins.Add(selectedSkin)) 
            { 
            visualToActivate = pawsVisualObject; 
            wonNewSkin = true; 
            skinWonName = selectedSkin; 
            }
        }

        if (!wonNewSkin) {
            if (noMatchVisualVariants != null && noMatchVisualVariants.Count > 0) {
                visualToActivate = noMatchVisualVariants[Random.Range(0, noMatchVisualVariants.Count)];
            } else { }
        }

        if (visualToActivate != null) 
        {
            visualToActivate.SetActive(true);
        }

        yield return new WaitForSeconds(winDisplayDelay);

        if (resultTextBackgroundObject != null)
        {
            resultTextBackgroundObject.SetActive(true);
        }

        if (resultText != null)
        {
            if (wonNewSkin && skinWonName != null)
            {
                resultText.text = $"You won {skinWonName}!";
            }
            else
            {
                resultText.text = "Try Again!";
            }
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


    private IEnumerator HideResultMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (resultTextBackgroundObject != null)
        {
            resultTextBackgroundObject.SetActive(false);
        }
    }
}