using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour{
    // Parameters for the Skin Selector Sprite
    public CharacterDatabase CharacterDB;
    public SpriteRenderer ArtworkSprite;
   
    private int selectedOption = 0;

    //Loads in the Skin selected
    void Start(){

        if(!PlayerPrefs.HasKey("SelectedOption")){
            selectedOption = 0;
        }else{
            Load();
        }
        UpdateCharacter(selectedOption);
    }

    //Updates sprite to the next skin
    public void NextOption(){
        selectedOption++;
        if(selectedOption >= CharacterDB.CharacterCount){
            selectedOption = 0;
        }
        UpdateCharacter(selectedOption);
    }

    //Update sprite to the previous skin
    public void BackOption(){
        selectedOption--;
        if(selectedOption < 0){
            selectedOption = CharacterDB.CharacterCount -1;
        }
        UpdateCharacter(selectedOption);
    }

    //Updates the Skin based on the database
    private void UpdateCharacter(int selectedOption){
        Character Character = CharacterDB.GetCharacter(selectedOption);
        ArtworkSprite.sprite = Character.CharacterSprite;
    }

    //Saves skin selected
    public void ApplyCharacter(){
        Save();
    }

    //Loads skin saved in player prefs
    private void Load(){
        selectedOption = PlayerPrefs.GetInt("SelectedOption");
    }

    //Save skin in player prefs
    private void Save(){
        PlayerPrefs.SetInt("SelectedOption",selectedOption);
    }
   
   //Changes scene
    public void ChangeScene(int scenceID){
        SceneManager.LoadScene(scenceID);
    }

}
