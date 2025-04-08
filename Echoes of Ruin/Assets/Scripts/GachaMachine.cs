using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GachaMachine : MonoBehaviour
{ 
    // Public sprite references for different skin rarities
    public Sprite spriteDefault, spriteRare1, spriteRare2, spriteSuperRare1, spriteSuperRare2, spriteUltraRare1;


    // Lists of skin names by rarity
    [SerializeField] 
    private List<string> rareSkins = new List<string>{"Rare1", "Rare2"};
    
    [SerializeField]
    private List<string> superRareSkins = new List<string>{"SuperRare1", "SuperRare2"};
    
    [SerializeField]
    private List<string> ultraSkins = new List<string>{"UltraRare1"};
    
    // Set of skins the player owns
    public HashSet<string> mySkins = new HashSet<string>(){"Default"};
    
    // UI Components
    [SerializeField]
    private TextMeshProUGUI resultText;
    
    [SerializeField]
    private Image skinDisplay;
    
    [SerializeField]
    private TextMeshProUGUI showProbability;

    // Dictionary to map skin names to their sprites
    public Dictionary<string, Sprite> skinSprites = new Dictionary<string, Sprite>();

    // Called when the script instance is being loaded
    private void Awake()
    {
        StartDictionary();
    }   

    // Initialize the dictionary that maps skin names to sprites
    private void StartDictionary()
    {
        skinSprites.Add("Default", spriteDefault);
        skinSprites.Add("Rare1", spriteRare1);
        skinSprites.Add("Rare2", spriteRare2);
        skinSprites.Add("SuperRare1", spriteSuperRare1);
        skinSprites.Add("SuperRare2", spriteSuperRare2);
        skinSprites.Add("UltraRare1", spriteUltraRare1);
    } 


    // Performs a gacha roll to potentially win a skin
    public void Roll()
    {
        // Determine the roll outcome (64 * 128 * 256 = 2097152)
        int roll = UnityEngine.Random.Range(1, 2097153);
        int rsRoll = UnityEngine.Random.Range(0, 2); // Roll for rare and super rare skins index
        int uRoll = UnityEngine.Random.Range(0, 1); // Roll for ultra rare skins index

        if (roll <= 32768) // 1 in 64 chance (2097151/64)
        {
            string selectedSkin = rareSkins[rsRoll];

            if (!mySkins.Contains(selectedSkin))
            {
                mySkins.Add(selectedSkin);
                skinDisplay.sprite = skinSprites[selectedSkin];
                skinDisplay.gameObject.SetActive(true);
                resultText.text = $"You won! {selectedSkin}";
                resultText.color = Color.black;
            }
            else 
            {
                resultText.text = "You Lost!";
                resultText.color = Color.black;
            }
        }
        else if (roll <= 32768 + 16384) // 1 in 128 chance (2097151/64 + 2097151/128)
        {
            string selectedSkin = superRareSkins[rsRoll];

            if (!mySkins.Contains(selectedSkin))
            {
                mySkins.Add(selectedSkin);
                skinDisplay.sprite = skinSprites[selectedSkin];
                skinDisplay.gameObject.SetActive(true);
                resultText.text = $"You won SuperRare Skin!! {selectedSkin}";
                resultText.color = Color.magenta;
            }
            else 
            { 
                resultText.text = "You Lost!";
                resultText.color = Color.black;
            }
        }
        else if (roll <= 32768 + 16384 + 8192) // 1 in 256 chance (2097151/64 + 2097151/128 + 2097151/256)
        {
            string selectedSkin = ultraSkins[uRoll];

            if (!mySkins.Contains(selectedSkin))
            {
                mySkins.Add(selectedSkin);
                skinDisplay.sprite = skinSprites[selectedSkin];
                skinDisplay.gameObject.SetActive(true);
                resultText.text = $"You won Ultra Skin!!! {selectedSkin}";
                resultText.color = Color.yellow;
            }
            else 
            {
                resultText.text = "You Lost!";
                resultText.color = Color.black;
            }
        }
        else 
        {
            resultText.text = "You Lost!";
            resultText.color = Color.black;
        }
    }

    // Displays the probability information for the gacha machine
    public void ViewProbability()
    {
        showProbability.text = "Gacha Machine Probabilities:\n\n" +
                  "- Rare Skin: 1 in 64 chance (1.56%)\n" +
                  "- Super Rare Skin: 1 in 128 chance (0.78%)\n" + 
                  "- Ultra Rare Skin: 1 in 256 chance (0.39%)\n" +
                  "- No Prize: 97.27% chance";
    }
}