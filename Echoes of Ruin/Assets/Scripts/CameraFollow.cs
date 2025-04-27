using UnityEngine;

public class CameraFollow : MonoBehaviour{
    public Transform Target;
    public Vector3 offSet = new Vector3(0, 0, -10);
    public float smoothSpeed = 0;


    //Updates based on Saved position
    void LateUpdate(){
        if (Target == null) return;
        if (Target != null) {
            transform.position = Target.position + offSet; 
         }

        Vector3 desiredPosition = Target.position + offSet;
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothed;
    }

    //Reduces the lag between scenes
    public void SnapToTarget(){
        if (Target != null){
            transform.position = Target.position + offSet;
        }
    }
}