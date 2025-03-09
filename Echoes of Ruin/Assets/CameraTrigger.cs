using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraTrigger : MonoBehaviour
{
    void onTriggerEnter2D(Collider2D other){
        if (other.CompareTag("PlayerCat"))
        {
            Debug.Log("Trigger Activated!");
        }
    }
    public Vector3 newCameraPos, newPlayerCatPos;
    CamControl cameraControl; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraControl = Camera.main.GetComponent<CamControl>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerCat") 
        {
            cameraControl.minPosi += newCameraPos;
            cameraControl.maxPosi += newCameraPos;
        }
        other.transform.position += newPlayerCatPos;
    }
}
