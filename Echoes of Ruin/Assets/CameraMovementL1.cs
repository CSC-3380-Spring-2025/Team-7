using UnityEngine;

public class CameraMovementL1 : MonoBehaviour
{
    public float camSpeed = 1f; //setting the camera speed
    public Transform targetPos; //targetPos will be the reference in which the camera will move with, such as playercat

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = new Vector3(targetPos.position.x, targetPos.position.y + 1, -10f); //new vector3 = new x y z positions
        transform.position = Vector3.Slerp(transform.position, newPosition, camSpeed); //during runtime, camera moves according to given xyz position of targetPos object such as playercat
        
    }
}
