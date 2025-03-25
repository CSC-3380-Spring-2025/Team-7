using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser : MonoBehaviour
{
    //set speed value in unity
    public float speed;

    void Start()
    {
        
    } 

    void Upadate ()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
   }
