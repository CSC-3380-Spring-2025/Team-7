<<<<<<< Updated upstream
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Collections;
=======
>>>>>>> Stashed changes
using UnityEngine;

public class CameraMovementL1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
<<<<<<< Updated upstream
    public float speedOfCam = 1f;
    public Transform targetPosition;
    public float heightCam = 1;
=======
    public float camSpeed = 1f;
    public Transform targetPos;
>>>>>>> Stashed changes

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        Vector3 newPosition = new Vector3(targetPosition.position.x, targetPosition.position.y+heightCam, -10f);
        transform.position = Vector3.Slerp(transform.position, newPosition, speedOfCam*Time.deltaTime);
=======
        Vector3 newPosition = new Vector3(targetPos.position.x, targetPos.position.y + 1, -10f);
        transform.position = Vector3.Slerp(transform.position, newPosition, camSpeed);
        
>>>>>>> Stashed changes
    }
}
