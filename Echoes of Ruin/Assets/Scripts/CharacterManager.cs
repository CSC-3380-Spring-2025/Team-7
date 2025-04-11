using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour{

    public CharacterDatabase characterDB;
    public SpriteRenderer artworkSprite;

    private int SelectedOption = 0;

    void Start(){
        UpdateCharacter(SelectedOption);
    }

    public void NextOption(){
        SelectedOption++;
        if(SelectedOption >= characterDB.characterCount){
            SelectedOption = 0;
        }
        UpdateCharacter(SelectedOption);
    }

    public void BackOption(){
        SelectedOption--;
        if(SelectedOption < 0){
            SelectedOption = characterDB.characterCount -1;
        }
        UpdateCharacter(SelectedOption);
    }

    private void UpdateCharacter(int SelectedOption){
        Character character = characterDB.getCharacter(SelectedOption);
        artworkSprite.sprite = character.characterSprite;
        
    }

   
    
}
