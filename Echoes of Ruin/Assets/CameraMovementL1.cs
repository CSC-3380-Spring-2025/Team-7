using UnityEngine;

public class CameraMovementL1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float camSpeed = 1f;
    public Transform targetPos;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = new Vector3(targetPos.position.x, targetPos.position.y + 1, -10f);
        transform.position = Vector3.Slerp(transform.position, newPosition, camSpeed);
        
    }
}
