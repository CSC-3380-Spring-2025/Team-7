using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScatterEnemy : MonoBehaviour
{
    public Transform player;  //refrense to player
    public GameObject scatter; //scatter bullets
    public int bulletCount = 5; //Number of bullets per shot
    public float spreadAngle = 45f; //Total spread angle of bullets
    private float shootCooldown;
    public float startShootCooldown = 2f;

    void Start()
    {
        shootCooldown = startShootCooldown;
    }

    //checks if cooldown is at 0 and shoots bullets if yes, if no then timer decreases  
    void Update()
    {
       if (shootCooldown <= 0) 
       {
        ShootBullets();
        shootCooldown = startShootCooldown;
       } else {
        shootCooldown -= Time.deltaTime;
       }
    }

    void ShootBullets()
    {
        float angleStep = spreadAngle / (bulletCount - 1); // Angle between bullets 
        float startAngle = -spreadAngle / 2; //Starting angle 

        for(int i = 0; i < bulletCount; i++)
        {
            float bulletAngle = startAngle + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + bulletAngle);

             Instantiate(scatter, transform.position, transform.rotation); // spawns bulltets at enemy location 
        }
    }
}

