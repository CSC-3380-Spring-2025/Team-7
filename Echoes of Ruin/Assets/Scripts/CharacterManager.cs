using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour{

    public CharacterDatabase characterDB;
    public SpriteRenderer artworkSprite;
   
    private int SelectedOption = 0;

    void Start(){

        if(!PlayerPrefs.HasKey("SelectedOption")){
            SelectedOption = 0;
        }else{
            Load();
        }
        UpdateCharacter(SelectedOption);
    }

    public void NextOption(){
        SelectedOption++;
        if(SelectedOption >= characterDB.characterCount){
            SelectedOption = 0;
        }
        UpdateCharacter(SelectedOption);
        Save();
    }

    public void BackOption(){
        SelectedOption--;
        if(SelectedOption < 0){
            SelectedOption = characterDB.characterCount -1;
        }
        UpdateCharacter(SelectedOption);
        Save();
    }

    private void UpdateCharacter(int SelectedOption){
        Character character = characterDB.getCharacter(SelectedOption);
        artworkSprite.sprite = character.characterSprite;
        
    }

    private void Load(){
        SelectedOption = PlayerPrefs.GetInt("SelectedOption");
    }

    private void Save(){
        PlayerPrefs.SetInt("SelectedOption",SelectedOption);
    }
   
    public void ChangeScene(int scenceID){
        SceneManager.LoadScene(scenceID);
    }

  
}
