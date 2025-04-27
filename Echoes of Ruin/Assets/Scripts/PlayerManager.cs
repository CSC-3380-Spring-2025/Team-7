using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour{
    public static PlayerManager Instance;

    public GameObject player;    
    private GameObject currentPlayer;
    private Vector3 savedPosition;

    public CharacterDatabase characterDB;


    private int SelectedOption;

    private void Awake() {
       
        if (Instance != null && Instance != this){
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
         player = Resources.Load<GameObject>("PlayerCat");

        currentPlayer = GameObject.FindWithTag("PlayerCat");
        if (currentPlayer != null){
        DontDestroyOnLoad(currentPlayer);
     }


        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start(){
        SelectedOption = PlayerPrefs.GetInt("SelectedOption", 0);
        if (currentPlayer == null || !PlayerPrefs.HasKey("SelectedOption")){
            SelectedOption = 0;
        }else{
            LoadPlayer();
        }
        UpdateCharacter(SelectedOption);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        SelectedOption = PlayerPrefs.GetInt("SelectedOption", 0);
        if (!PlayerPrefs.HasKey("SelectedOption")){
            SelectedOption = 0;
        }else{
            SelectedOption = PlayerPrefs.GetInt("SelectedOption", 0);
        }

        if(currentPlayer == null){
             currentPlayer = GameObject.FindWithTag("PlayerCat");
            if (currentPlayer != null){
               DontDestroyOnLoad(currentPlayer);
            }
        UpdateCharacter(SelectedOption);
        }

        
    }

    public void LoadPlayer(){
        if (player == null) return;
        if (currentPlayer != null){
            Destroy(currentPlayer);
        }

        DontDestroyOnLoad(currentPlayer);
        SelectedOption = PlayerPrefs.GetInt("SelectedOption",0);
        UpdateCharacter(SelectedOption);
      
    }

    private void KeepPosition(){
        if (currentPlayer != null){
            savedPosition = currentPlayer.transform.position;
        }
    }

     private void UpdateCharacter(int SelectedOption){
        Character character = characterDB.getCharacter(SelectedOption);
        if (character == null || currentPlayer == null) return;
        SpriteRenderer sprite = currentPlayer.GetComponentInChildren<SpriteRenderer>();
        Animator animator = currentPlayer.GetComponentInChildren<Animator>();
        if (sprite != null && character.characterSprite != null){
        sprite.sprite = character.characterSprite;
        }
        if (animator != null && character.animatorOverride != null){
        animator.runtimeAnimatorController = character.animatorOverride;
        }
    }

    public void SetSelectedCharacter(int index) {
        SelectedOption = index;
        PlayerPrefs.SetInt("SelectedOption", index);
        PlayerPrefs.Save();

        UpdateCharacter(index);

  
    }
}