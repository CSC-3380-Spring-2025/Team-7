using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelector : MonoBehaviour
{
    public DataCharacters characterDB;
    public PlayerSkinApplier playerSkinApplier;
    private GachaMachine gachaMachine;
    public Image selectedCharacterDisplay;
    public Button applyButton;
    public Button nextButton;
    public Button prevButton;
    private int currentSelectionIndex = 0;
    private DataCharacterEntry currentlyDisplayedCharacter;

    void Start()
    {
        gachaMachine = FindAnyObjectByType<GachaMachine>();
        if (gachaMachine == null)
        {
            DisableInteraction();
            return;
        }

        if (characterDB == null)
        {
            DisableInteraction();
            return;
        }

        if (playerSkinApplier == null) {
            if (applyButton != null) applyButton.gameObject.SetActive(false);
        }

        if (characterDB.CharacterCount == 0)
        {
             DisableInteraction();
             return;
        }

        if (nextButton != null) nextButton.onClick.AddListener(NextCharacter); else DisableInteraction();
        if (prevButton != null) prevButton.onClick.AddListener(PreviousCharacter); else DisableInteraction();
        if (applyButton != null) applyButton.onClick.AddListener(ApplySelectedSkin); 

        currentSelectionIndex = FindStartingIndex();
        UpdateCharacterDisplay();
    }

    void NextCharacter()
    {
        currentSelectionIndex++;
        if (currentSelectionIndex >= characterDB.CharacterCount)
        {
            currentSelectionIndex = 0;
        }
        UpdateCharacterDisplay();
    }

    void PreviousCharacter()
    {
        currentSelectionIndex--;
        if (currentSelectionIndex < 0)
        {
            currentSelectionIndex = characterDB.CharacterCount - 1;
        }
        UpdateCharacterDisplay();
    }

    void UpdateCharacterDisplay()
    {
        currentlyDisplayedCharacter = characterDB.GetCharacter(currentSelectionIndex);

        if (currentlyDisplayedCharacter == null)
        {
            if(selectedCharacterDisplay != null) selectedCharacterDisplay.enabled = false;
            if(applyButton != null) applyButton.gameObject.SetActive(false);
            return;
        }

        string skinName = currentlyDisplayedCharacter.characterName;
        Sprite skinSprite = currentlyDisplayedCharacter.characterDisplaySprite;

        if (selectedCharacterDisplay != null)
        {
            selectedCharacterDisplay.sprite = skinSprite;
            selectedCharacterDisplay.enabled = (skinSprite != null);

            if (selectedCharacterDisplay != null) {
                selectedCharacterDisplay.gameObject.SetActive(false);
                selectedCharacterDisplay.gameObject.SetActive(true);
            }
        }

        if (applyButton != null)
        {
            if (gachaMachine != null)
            {
                bool isOwned = gachaMachine.mySkins.Contains(skinName);
                applyButton.gameObject.SetActive(isOwned);
            }
            else
            {
                if(applyButton != null) applyButton.gameObject.SetActive(false);
            }
        }
    }

    void DisableInteraction()
    {
        if (nextButton != null) nextButton.interactable = false;
        if (prevButton != null) prevButton.interactable = false;
        if (applyButton != null) applyButton.gameObject.SetActive(false);
    }

    int FindStartingIndex()
    {
        if (characterDB == null || characterDB.CharacterCount == 0) return 0;

        for (int i = 0; i < characterDB.CharacterCount; i++)
        {
            if (characterDB.GetCharacter(i)?.characterName == "Default")
            {
                return i;
            }
        }
        return 0;
    }

    public void ApplySelectedSkin()
    {
        if (currentlyDisplayedCharacter == null) { return; }
        if (playerSkinApplier == null) { return; }
        if (gachaMachine == null) { return; }

        string skinToApply = currentlyDisplayedCharacter.characterName;

        if (!gachaMachine.mySkins.Contains(skinToApply)) {
            return;
        }

        bool applied = playerSkinApplier.ActivateSkin(skinToApply);

        if (applied) {
            PlayerPrefs.SetString("LastSelectedSkin", skinToApply);
            PlayerPrefs.Save();
        }
    }
}
