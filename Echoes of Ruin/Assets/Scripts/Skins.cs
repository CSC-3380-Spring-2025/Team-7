using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinChanger : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] private SpriteRenderer playerSprite;

    private GachaMachine gachaMachine;
    private string currentSkinName = "Default";

    void Start()
    {
        gachaMachine = FindAnyObjectByType<GachaMachine>();

        if (gachaMachine == null)
        {
            ApplyDefaultVisuals();
            return;
        }

        string savedSkin = PlayerPrefs.GetString("LastSelectedSkin", "Default");
        ApplySkinVisuals(savedSkin);
    }

    public bool ApplySkinVisuals(string skinName)
    {
        if (playerSprite == null)
        {
            return false;
        }

        if (gachaMachine == null)
        {
             gachaMachine = FindAnyObjectByType<GachaMachine>();
             if (gachaMachine == null) {
                ApplyDefaultVisuals();
                return false;
             }
        }

        if (skinName == "Default" || gachaMachine.mySkins.Contains(skinName))
        {
            if (gachaMachine.skinSprites.ContainsKey(skinName))
            {
                playerSprite.sprite = gachaMachine.skinSprites[skinName];
                this.currentSkinName = skinName;
                return true;
            }
            else
            {
                ApplyDefaultVisuals();
                return false;
            }
        }
        else
        {
            ApplyDefaultVisuals();
            return false;
        }
    }

    private void ApplyDefaultVisuals()
    {
        if (playerSprite == null) return;

        if (gachaMachine != null && gachaMachine.skinSprites.ContainsKey("Default"))
        {
            playerSprite.sprite = gachaMachine.skinSprites["Default"];
            this.currentSkinName = "Default";
        }
    }

    public string GetCurrentSkinName()
    {
        return currentSkinName;
    }

    public void SetCurrentSkin(string skinName)
    {
       ApplySkinVisuals(skinName);
    }
}