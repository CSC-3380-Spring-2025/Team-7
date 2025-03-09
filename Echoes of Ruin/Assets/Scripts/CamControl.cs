using UnityEngine;

public class CamControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform PlayerCat;
    public float camSpeed;

    private Vector3 targetCamPos, newCamPos;
    public Vector3 minPosi, maxPosi;

    // Update is called once per frame
    void LateUpdate()
    {
        if (transform.position != PlayerCat.position)
        {
            targetCamPos = PlayerCat.position;
            Vector3 camBoundary = new Vector3
            (
                Mathf.Clamp(targetCamPos.x, minPosi.x, maxPosi.x),
                Mathf.Clamp(targetCamPos.y, minPosi.y, maxPosi.y),
                Mathf.Clamp(targetCamPos.z, minPosi.z, maxPosi.z));

            newCamPos = Vector3.Lerp(transform.position, camBoundary, camSpeed);
            transform.position = newCamPos;
        }
    }
}
