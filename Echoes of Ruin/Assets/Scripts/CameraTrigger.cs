using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraTrigger : MonoBehaviour
{
    public Vector3 newCameraPos; //when trigger hits between rooms, camera has to move certain points forward/lower
    public Vector3 newPlayerCatPos; //new player position
    CamControl cameraControl; //calls on CamControl script;
    void Start()
    {
        cameraControl = Camera.main.GetComponent<CamControl>(); //calls onto cameracontrol
    }

    private void OnTriggerEnter2D(Collider2D obj) //upon trigger of playercat
    {
        if (obj.gameObject.tag == "PlayerCat") //if it is playercat
        {
            cameraControl.minPosi += newCameraPos; //camera will move from current pos to the amount set in unity
            cameraControl.maxPosi += newCameraPos; //camera will move from current pos to the amount set in unity
        }
        obj.transform.position += newPlayerCatPos; //playercat will also move
    }
}
