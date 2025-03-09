using UnityEngine;
using System.Collections;
using System.Collections.Generic; 


public class wasdMovement : MonoBehaviour
{
    public float speed = 9.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        Vector3 position1 = transform.position;
        if (Input.GetKey("w"))
        {
            position1.y += speed * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            position1.x -= speed * Time.deltaTime;
        }

        if (Input.GetKey("s"))
        {
            position1.y -= speed * Time.deltaTime;
        }

        if (Input.GetKey("d"))
        {
            position1.x += speed * Time.deltaTime;
        }
        
        transform.position = position1;
    }
}
