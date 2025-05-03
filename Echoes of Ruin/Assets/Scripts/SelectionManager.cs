using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Handles UI logic for selecting character skins in the CharacterSelection scene.
public class CharacterSelector : MonoBehaviour
{
    // Reference to the ScriptableObject containing character definitions.
    public DataCharacters characterDB;
    // Reference to the GachaMachine to check owned skins. (Removed PlayerSkinApplier ref)
    // public PlayerSkinApplier playerSkinApplier; // No longer needed here
    private GachaMachine gachaMachine;

    // UI element references.
    public Image selectedCharacterDisplay;
    public Button applyButton;
    public Button nextButton;
    public Button prevButton;

    private int currentSelectionIndex = 0;
    private DataCharacterEntry currentlyDisplayedCharacter;

    // Initializes references and UI state.
    void Start()
    {
        // Find persistent GachaMachine instance.
        gachaMachine = GachaMachine.Instance; // Use Singleton access
        if (gachaMachine == null)
        {
             // Disable if GachaMachine isn't running.
             Debug.LogError("[CharacterSelector] GachaMachine Instance not found!");
            DisableInteraction();
            return;
        }

        if (characterDB == null || characterDB.CharacterCount == 0)
        {
             Debug.LogError("[CharacterSelector] CharacterDB not assigned or is empty!");
             DisableInteraction();
             return;
        }

        // Add listeners to buttons if they exist.
        if (nextButton != null) nextButton.onClick.AddListener(NextCharacter); else DisableInteraction();
        if (prevButton != null) prevButton.onClick.AddListener(PreviousCharacter); else DisableInteraction();
        if (applyButton != null) applyButton.onClick.AddListener(ApplySelectedSkin); else DisableInteraction();

        // Find the starting character (e.g., "Default") and display it.
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
        if (currentlyDisplayedCharacter == null) // Handle invalid index gracefully.
        {
            if(selectedCharacterDisplay != null) selectedCharacterDisplay.enabled = false;
            if(applyButton != null) applyButton.gameObject.SetActive(false);
            return;
        }

        string skinName = currentlyDisplayedCharacter.characterName;
        Sprite skinSprite = currentlyDisplayedCharacter.characterDisplaySprite;

        // Update the display image.
        if (selectedCharacterDisplay != null)
        {
            selectedCharacterDisplay.sprite = skinSprite;
            selectedCharacterDisplay.enabled = (skinSprite != null);
             // Optional: Force UI refresh if needed (usually not necessary)
             // selectedCharacterDisplay.gameObject.SetActive(false);
             // selectedCharacterDisplay.gameObject.SetActive(true);
        }

        // Show/hide Apply button based on ownership.
        if (applyButton != null)
        {
            // Assume gachaMachine is valid due to Start() check.
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
        // Removed PlayerPrefs loading here, let PlayerSkinApplier handle the default load
        if (characterDB == null || characterDB.CharacterCount == 0) return 0;
        for (int i = 0; i < characterDB.CharacterCount; i++)
        {
            if (characterDB.GetCharacter(i)?.characterName == "Default") { return i; }
        }
        return 0; // Default to first character if "Default" isn't found
    }

    // Called when the Apply button is clicked. Saves the selection.
    public void ApplySelectedSkin()
    {
        if (currentlyDisplayedCharacter == null) { return; }
        if (gachaMachine == null) { return; } // Should not happen if Start succeeded

        string skinToApply = currentlyDisplayedCharacter.characterName;

        // Double-check ownership before saving.
        if (!gachaMachine.mySkins.Contains(skinToApply)) {
            Debug.LogWarning($"[CharacterSelector] Tried to apply unowned skin: {skinToApply}");
            return;
        }

        // --- ONLY SAVE THE CHOICE TO PLAYERPREFS ---
        PlayerPrefs.SetString("LastSelectedSkin", skinToApply);
        PlayerPrefs.Save();
        Debug.Log($"[CharacterSelector] Saved skin selection: {skinToApply}");
        // --- DO NOT CALL PlayerSkinApplier.SetCurrentSkin here ---

        // Optional: Provide feedback to the player that the skin was selected/saved.
        // e.g., change button text, play a sound.
    }
}