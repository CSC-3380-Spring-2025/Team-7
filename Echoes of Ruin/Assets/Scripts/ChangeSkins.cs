using UnityEngine;
using UnityEngine.UI;

public class ChangeSkins : MonoBehaviour {

    //All the cat override animators
    public AnimatorOverrideController DefCatAnimator;
    public AnimatorOverrideController BlackCatAnimator;
    public AnimatorOverrideController WhiteCatAnimator;
    public AnimatorOverrideController SiameseCatAnimator;
    public AnimatorOverrideController AshortCatAniamtor;
    public AnimatorOverrideController CalicoCatAnimator;

    //parameters for the player
    public CharacterDatabase CharacterDB;
    public Animator Animator;
    public RuntimeAnimatorController NewAnimator;

    private int selectedOption;

    //Animator Methods
    public void DefCat() {
        GetComponent<Animator>().runtimeAnimatorController = DefCatAnimator as RuntimeAnimatorController;
    }

     public void BlackCat() {
        GetComponent<Animator>().runtimeAnimatorController = BlackCatAnimator as RuntimeAnimatorController;
    }

     public void WhiteCat() {
        GetComponent<Animator>().runtimeAnimatorController = WhiteCatAnimator as RuntimeAnimatorController;
    }
    
      public void SiameseCat() {
        GetComponent<Animator>().runtimeAnimatorController = SiameseCatAnimator as RuntimeAnimatorController;
    }

     public void AShortCat() {
        GetComponent<Animator>().runtimeAnimatorController = AshortCatAniamtor as RuntimeAnimatorController;
    }

     public void CalicoCat() {
        GetComponent<Animator>().runtimeAnimatorController = CalicoCatAnimator as RuntimeAnimatorController;
    }

}
