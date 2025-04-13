using UnityEngine;
using UnityEngine.UI;

public class ChangeSkins : MonoBehaviour{

    public AnimatorOverrideController defCat;
    public AnimatorOverrideController blackCat;
    public AnimatorOverrideController whiteCat;
    public AnimatorOverrideController siameseCat;
    public AnimatorOverrideController ashortCat;
    public AnimatorOverrideController calicoCat;

    public CharacterDatabase characterDB;
    public Animator animator;
    public RuntimeAnimatorController newAnimator;

    private int SelectedOption;
/*
    void Start(){
      SelectedOption = PlayerPrefs.GetInt("SelectedOption",0);
      Character character = characterDB.getCharacter(SelectedOption);


      if(animator != null ){
        animator.runtimeAnimatorController = newAnimator;
      }
    }
*/

    public void DefCat(){
        GetComponent<Animator>().runtimeAnimatorController = defCat as RuntimeAnimatorController;
    }

     public void BlackCat(){
        GetComponent<Animator>().runtimeAnimatorController = blackCat as RuntimeAnimatorController;
    }

     public void WhiteCat(){
        GetComponent<Animator>().runtimeAnimatorController = whiteCat as RuntimeAnimatorController;
    }
    
      public void SiameseCat(){
        GetComponent<Animator>().runtimeAnimatorController = siameseCat as RuntimeAnimatorController;
    }

     public void AShortCat(){
        GetComponent<Animator>().runtimeAnimatorController = ashortCat as RuntimeAnimatorController;
    }

     public void CalicoCat(){
        GetComponent<Animator>().runtimeAnimatorController = calicoCat as RuntimeAnimatorController;
    }

}
