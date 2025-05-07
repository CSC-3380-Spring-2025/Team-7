using UnityEngine;

public class CameraFollow : MonoBehaviour{
    public Transform Target; //what object the camera is supposed to follow
    public Vector3 offSet = new Vector3(0, 0, -10); //initiliazes camera to central position
    public float smoothSpeed = 0; //dictates camera speed


    void LateUpdate(){
        if (Target != null) 
        {
            transform.position = Target.position + offSet; //will transform to whatever object Target is set to such as player
         }

        Vector3 desiredPosition = Target.position + offSet; 
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); 
        transform.position = smoothed;
    }

    public void SnapToTarget(){
        if (Target != null){
            transform.position = Target.position + offSet;
        }
    }
}