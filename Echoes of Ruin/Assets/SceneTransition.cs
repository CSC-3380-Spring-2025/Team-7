using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    public int sceneBuildIndex; //gets the index that is set by unity for each different scene

    private void OnTriggerEnter2D(Collider2D obj) //if player enters trigger area
    {
        if(obj.tag == "PlayerCat"){ //if it's player
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single); //loads to a new scene, the scene number is changeable in unity but it will transition to that scene from the current scene
        }
    }
}
