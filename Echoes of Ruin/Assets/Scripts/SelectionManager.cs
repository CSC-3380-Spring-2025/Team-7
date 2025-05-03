using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CharacterSelector : MonoBehaviour
{

    public DataCharacters characterDB;
    private GachaMachine gachaMachine;
    public Image selectedCharacterDisplay;
    public Button applyButton;
    public Button nextButton;
    public Button prevButton;
    private int currentSelectionIndex = 0;
    private DataCharacterEntry currentlyDisplayedCharacter;

    void Start()
    {
        gachaMachine = GachaMachine.Instance;
        if (gachaMachine == null) { DisableInteraction(); return; }
        if (characterDB == null || characterDB.CharacterCount == 0) { DisableInteraction(); return; }

        if (nextButton != null) nextButton.onClick.AddListener(NextCharacter); else DisableInteraction();
        if (prevButton != null) prevButton.onClick.AddListener(PreviousCharacter); else DisableInteraction();
        if (applyButton != null) applyButton.onClick.AddListener(ApplySelectedSkin); else DisableInteraction();

        currentSelectionIndex = FindStartingIndex();
        UpdateCharacterDisplay();
    }

    // Cycles to the next character in the database.
    void NextCharacter()
    {
        currentSelectionIndex++;
        if (currentSelectionIndex >= characterDB.CharacterCount) { currentSelectionIndex = 0; }
        UpdateCharacterDisplay();
    }

    // Cycles to the previous character in the database.
    void PreviousCharacter()
    {
        currentSelectionIndex--;
        if (currentSelectionIndex < 0) { currentSelectionIndex = characterDB.CharacterCount - 1; }
        UpdateCharacterDisplay();
    }

    // Updates the displayed character sprite and Apply button visibility.
    void UpdateCharacterDisplay()
    {
        currentlyDisplayedCharacter = characterDB.GetCharacter(currentSelectionIndex);
        if (currentlyDisplayedCharacter == null) {
            if(selectedCharacterDisplay != null) selectedCharacterDisplay.enabled = false;
            if(applyButton != null) applyButton.gameObject.SetActive(false);
            return;
        }
        string skinName = currentlyDisplayedCharacter.characterName;
        Sprite skinSprite = currentlyDisplayedCharacter.characterDisplaySprite;
        if (selectedCharacterDisplay != null) {
            selectedCharacterDisplay.sprite = skinSprite;
            selectedCharacterDisplay.enabled = (skinSprite != null);
        }
        if (applyButton != null) {
            bool isOwned = gachaMachine.mySkins.Contains(skinName);
            applyButton.gameObject.SetActive(isOwned);
        }
    }

    // Disables interaction buttons.
    void DisableInteraction()
    {
        if (nextButton != null) nextButton.interactable = false;
        if (prevButton != null) prevButton.interactable = false;
        if (applyButton != null) applyButton.gameObject.SetActive(false);
    }

    // Finds the index of the "Default" character, or returns 0.
    int FindStartingIndex()
    {
        if (characterDB == null || characterDB.CharacterCount == 0) return 0;
        for (int i = 0; i < characterDB.CharacterCount; i++) {
            if (characterDB.GetCharacter(i)?.characterName == "Default") { return i; }
        }
        return 0;
    }

    // Called when the Apply button is clicked. Records the choice for the current session.
    public void ApplySelectedSkin()
    {
        if (currentlyDisplayedCharacter == null) { return; }
        if (gachaMachine == null) { return; }

        string skinToApply = currentlyDisplayedCharacter.characterName;

        if (!gachaMachine.mySkins.Contains(skinToApply)) { return; }
 
        PlayerSkinApplierFromSave.SelectSkinForSession(skinToApply);
        
   
    }
}