using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour{
    public CharacterDatabase characterDB;
    public SpriteRenderer artworkSprite;
    public Animator animator;
    public Transform playerTransform;

    private float xPos;
    private float yPos;

    private static Player instance;


    private int SelectedOption = 0;

    // Loading in Character
    void Start(){
        if(!PlayerPrefs.HasKey("SelectedOption")){
            SelectedOption = 0;
        }else{
            Load();
        }
        UpdateCharacter(SelectedOption);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 1){
            Destroy(gameObject); 
            return;
        }
    }

    void Update(){
        if (this != instance || playerTransform == null) return;
        KeepPosition();
    }

   private void UpdateCharacter(int SelectedOption){
        Character character = characterDB.getCharacter(SelectedOption);
        artworkSprite.sprite = character.characterSprite;
        animator.runtimeAnimatorController = character.animatorOverride;
        
    }

    private void Load(){
        SelectedOption = PlayerPrefs.GetInt("SelectedOption");
    }

    //Keep Position between scenes
    private void KeepPosition(){
        if (playerTransform != null) {
            xPos = playerTransform.position.x;
            yPos = playerTransform.position.y;
        }else{
            return;
        }
    
    }

   void Awake(){
    
    string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
        if (currentScene != "MainMenu" && currentScene != "CharacterSelection"){
            if (instance == null){
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else{
                Destroy(gameObject); 
                return;
            }
        }
    }

    void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if(!PlayerPrefs.HasKey("SelectedOption")){
            SelectedOption = 0;
        }else{
            Load();
        }
        UpdateCharacter(SelectedOption);
        
        Camera cam = Camera.main;
        if (cam != null){
            CameraFollow follow = cam.GetComponent<CameraFollow>();
            if (follow != null){
                follow.target = this.transform;
                follow.SnapToTarget();
            }
        }
        if (this == instance) {
            transform.position = new Vector3(xPos, yPos, transform.position.z);
        }
    }
}

