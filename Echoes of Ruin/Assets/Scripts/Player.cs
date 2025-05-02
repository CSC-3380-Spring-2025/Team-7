using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour{


   //Parameters for the player
   public CharacterDatabase CharacterDB;
   public SpriteRenderer ArtworkSprite;
   public Animator Animator;
   public Transform PlayerTransform;


   //Positions
   private float xPos;
   private float yPos;
   private static Vector3 savedPosition;
   private static Player instance;
   private static bool hasSavedPosition = false;
   private string currentSceneName;


   //Skin selection
   private int selectedOption = 0;


   // Loading in Player with skin selected.
   void Start(){
       currentSceneName = SceneManager.GetActiveScene().name;
       //if(!PlayerPrefs.HasKey("SelectedOption")){
       //    selectedOption = 0;
       //}else{
           //Load();
       //}
       //UpdateCharacter(selectedOption);


       GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerCat");
       if(currentSceneName == "CharacterSelection"){
           gameObject.SetActive(false);
       }


       if (players.Length > 1){
           return;
       }
   }


   //Updates position
   void Update(){
       if (this == null || PlayerTransform == null) return;
       if (this != instance || PlayerTransform == null) return;
       savedPosition = PlayerTransform.position;
       hasSavedPosition = true;
   }


   //Updates player skin and animator
   //private void UpdateCharacter(int selectedOption){
   //    Character character = CharacterDB.GetCharacter(selectedOption);
   //    ArtworkSprite.sprite = character.CharacterSprite;
   //    Animator.runtimeAnimatorController = character.AnimatorOverride;
   //}


   //Loads player pref skin
   //private void Load(){
   //    selectedOption = PlayerPrefs.GetInt("SelectedOption");
   //}


   //Keep Position between scenes
   private void KeepPosition(){
       if (PlayerTransform != null) {
           xPos = PlayerTransform.position.x;
           yPos = PlayerTransform.position.y;
       }else{
           return;
       }
   }


   //On awake it checks if there are dups of the cat player
   void Awake(){
   string currentScene = SceneManager.GetActiveScene().name;
       if (currentScene == "ForestClearing" ){
           if (instance == null){
               instance = this;
           }
           else{
               Destroy(gameObject);
           }
       }
   }


   //enable on scene loaded
   void OnEnable(){
       SceneManager.sceneLoaded += OnSceneLoaded;
   }


   //disabled  on scene loaded
   void OnDisable(){
       SceneManager.sceneLoaded -= OnSceneLoaded;
   }


   //On Scene Load it applys the skin, position, destorys dups, and reassigns the camera
   void OnSceneLoaded(Scene scene, LoadSceneMode mode){
       //Deletes dups in tutortialscene and resets position
       if (scene.name == "TutorialScene"){
           if (this == instance){
               Destroy(gameObject); 
           }
           savedPosition.x = 0;
           savedPosition.y = 0;
       }
       //Resets Position if scene changes to HomeScreen
       if(scene.name == "Homescreen"){
           savedPosition.x = 0;
           savedPosition.y = 0;
       }
       //if(!PlayerPrefs.HasKey("SelectedOption")){
       //    selectedOption = 0;
       //}else{
           //Load();
       //}
       //UpdateCharacter(selectedOption);
      
       Camera cam = Camera.main;
       if (cam != null){
           CameraFollow follow = cam.GetComponent<CameraFollow>();
           if (follow != null){
               follow.Target = this.transform;
               follow.SnapToTarget();
           }
       }
       if (this == instance && hasSavedPosition){
           transform.position = new Vector3(savedPosition.x, savedPosition.y, transform.position.z);
       }


   }
}





