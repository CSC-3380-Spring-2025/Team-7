using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GachaMachine : MonoBehaviour
{ 
    // Public sprite references for different skin rarities
    public Sprite spriteRare1, spriteRare2, spriteRare3, spriteRare4, spriteRare5, spriteRare6, 
        spriteSuperRare1, spriteSuperRare2, spriteSuperRare3, spriteSuperRare4, spriteSuperRare5, spriteSuperRare6, 
        spriteUltraRare1, spriteUltraRare2, spriteUltraRare3;


    // Lists of skin names by rarity
    [SerializeField] 
    private List<string> rareSkins = new List<string>{"Rare1", "Rare2", "Rare3", "Rare4", "Rare5", "Rare6"};
    
    [SerializeField]
    private List<string> superRareSkins = new List<string>{"SuperRare1", "SuperRare2", "SuperRare3", "SuperRare4", "SuperRare5", "SuperRare6"};
    
    [SerializeField]
    private List<string> ultraSkins = new List<string>{"UltraRare1", "UltraRare2", "UltraRare3"};
    
    // Set of skins the player owns
    private HashSet<string> mySkins = new HashSet<string>();
    
    // UI Components
    [SerializeField]
    private TextMeshProUGUI resultText;
    
    [SerializeField]
    private Image skinDisplay;
    
    [SerializeField]
    private TextMeshProUGUI showProbability;

    // Dictionary to map skin names to their sprites
    private Dictionary<string, Sprite> skinSprites = new Dictionary<string, Sprite>();

    /// <summary>
    /// Called when the script instance is being loaded
    /// </summary>
    private void Awake()
    {
        StartDictionary();
    }   

    /// <summary>
    /// Initialize the dictionary that maps skin names to sprites
    /// </summary>
    private void StartDictionary()
    {
        skinSprites.Add("Rare1", spriteRare1);
        skinSprites.Add("Rare2", spriteRare2);
        skinSprites.Add("Rare3", spriteRare3);
        skinSprites.Add("Rare4", spriteRare4);
        skinSprites.Add("Rare5", spriteRare5);
        skinSprites.Add("Rare6", spriteRare6);
        skinSprites.Add("SuperRare1", spriteSuperRare1);
        skinSprites.Add("SuperRare2", spriteSuperRare2);
        skinSprites.Add("SuperRare3", spriteSuperRare3);
        skinSprites.Add("SuperRare4", spriteSuperRare4);
        skinSprites.Add("SuperRare5", spriteSuperRare5);
        skinSprites.Add("SuperRare6", spriteSuperRare6);
        skinSprites.Add("UltraRare1", spriteUltraRare1);
        skinSprites.Add("UltraRare2", spriteUltraRare2);
        skinSprites.Add("UltraRare3", spriteUltraRare3);
    } 

    /// <summary>
    /// Performs a gacha roll to potentially win a skin
    /// </summary>
    public void Roll()
    {
        // Determine the roll outcome (64 * 128 * 256 = 2097152)
        int roll = UnityEngine.Random.Range(1, 2097153);
        int rsRoll = UnityEngine.Random.Range(0, 6); // Roll for rare and super rare skins index
        int uRoll = UnityEngine.Random.Range(0, 3); // Roll for ultra rare skins index

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

    /// <summary>
    /// Displays the probability information for the gacha machine
    /// </summary>
    public void ViewProbability()
    {
        showProbability.text = "Gacha Machine Probabilities:\n\n" +
                  "- Rare Skin: 1 in 64 chance (1.56%)\n" +
                  "- Super Rare Skin: 1 in 128 chance (0.78%)\n" + 
                  "- Ultra Rare Skin: 1 in 256 chance (0.39%)\n" +
                  "- No Prize: 97.27% chance";
    }
}