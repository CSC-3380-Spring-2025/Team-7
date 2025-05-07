using UnityEngine;
using System.Collections;
using System.Collections.Generic; 


public class WasdMovement : MonoBehaviour
{
    public float speed = 9.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        Vector3 position1 = transform.position;
        if (Input.GetKey("w")) //if press w
        {
            position1.y += speed * Time.deltaTime; //change position of y
        }

        if (Input.GetKey("a")) //if press a 
        {
            position1.x -= speed * Time.deltaTime;
        }

        if (Input.GetKey("s")) //if press s
        {
            position1.y -= speed * Time.deltaTime;
        }

        if (Input.GetKey("d")) //if press d in game
        {
            position1.x += speed * Time.deltaTime;
        }
        
        transform.position = position1; //changes it to the new position upon key press
    }
}
