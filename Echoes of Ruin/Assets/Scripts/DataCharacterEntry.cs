using UnityEngine; 

[System.Serializable] 
public class DataCharacterEntry
{
    [Tooltip("IMPORTANT: Must EXACTLY match the name used in GachaMachine skin lists AND dictionary keys!")]
    public string characterName;

    [Tooltip("The sprite to show in the Character Selection UI.")]
    public Sprite characterDisplaySprite;

    [Tooltip("Assign the specific Animator Controller asset for this skin (e.g., Player, ComBlackCatController).")]
    public RuntimeAnimatorController animatorController;
}