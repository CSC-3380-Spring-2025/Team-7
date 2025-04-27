using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour{
    public CharacterDatabase characterDB;
    public SpriteRenderer artworkSprite;
    public Animator animator;
    public Transform playerTransform;

    private float xPos;
    private float yPos;

      private static Vector3 savedPosition;

    private static Player instance;

     private static bool hasSavedPosition = false;

    string currentSceneName = SceneManager.GetActiveScene().name;

    private int SelectedOption = 0;

    // Loading in Character
    void Start(){
        if(!PlayerPrefs.HasKey("SelectedOption")){
            SelectedOption = 0;
        }else{
            Load();
        }
        UpdateCharacter(SelectedOption);

        GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerCat");
        if(currentSceneName == "Nurture" || currentSceneName == "CharacterSelection"){
            gameObject.SetActive(false);
        }

        if (players.Length > 1){
            Debug.Log("BIAFUWFHI");
            //Destroy(gameObject); 
            return;
        }
    }

    void Update(){
        if (this != instance || playerTransform == null) return;
        savedPosition = playerTransform.position;
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
    
    string currentScene = SceneManager.GetActiveScene().name;
        
        if (currentScene == "ForestClearing" ){
            if (instance == null){
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else{
                Destroy(gameObject); 
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
        if (scene.name == "TutorialScene"){
            if (this == instance){
                Destroy(gameObject);  
            }
        return;
        }
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
        if (this == instance && hasSavedPosition){
            transform.position = new Vector3(savedPosition.x, savedPosition.y, transform.position.z);
        }
    }
         
   }

