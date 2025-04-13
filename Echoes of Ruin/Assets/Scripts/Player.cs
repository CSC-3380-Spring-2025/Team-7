using UnityEngine;

public class Player : MonoBehaviour{
    public CharacterDatabase characterDB;
    public SpriteRenderer artworkSprite;
    public Animator animator;

    private int SelectedOption = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        if(!PlayerPrefs.HasKey("SelectedOption")){
            SelectedOption = 0;
        }else{
            Load();
        }
        UpdateCharacter(SelectedOption);
    }

   private void UpdateCharacter(int SelectedOption){
        Character character = characterDB.getCharacter(SelectedOption);
        artworkSprite.sprite = character.characterSprite;
        animator.runtimeAnimatorController = character.animatorOverride;
        
    }

    private void Load(){
        SelectedOption = PlayerPrefs.GetInt("SelectedOption");
    }
}
