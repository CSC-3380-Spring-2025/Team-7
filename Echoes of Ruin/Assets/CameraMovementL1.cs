using Unity.VisualScripting;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CameraMovementL1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speedOfCam = 1f;
    public Transform targetPosition;
    public float heightCam = 1;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = new Vector3(targetPosition.position.x, targetPosition.position.y+heightCam, -10f);
        transform.position = Vector3.Slerp(transform.position, newPosition, speedOfCam*Time.deltaTime);
    }
}
