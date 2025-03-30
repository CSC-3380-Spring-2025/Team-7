using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserEnemy : MonoBehaviour
{
    public Transform player;
    public GameObject laser;
    
    [SerializeField] private float startShootCooldown = 2f;
    [SerializeField] private float shootingRange = 10f; // Max distance to shoot
    [SerializeField] private LayerMask obstacleLayer;  // Layer for obstacles

    private float shootCooldown;

    void Start()
    {
        shootCooldown = startShootCooldown;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootingRange)
        {
            AimAtPlayer();
            Shoot();
        }

        // Reduce cooldown timer
        if (shootCooldown > 0)
            shootCooldown -= Time.deltaTime;
    }

// Rotates the enemy to face the player 
    private void AimAtPlayer()
    {
        Vector2 direction = player.position - transform.position;
        transform.up = direction;
    }

//Shoots when colldown reaches 0
    private void Shoot()
    {
        if (shootCooldown <= 0)
        {
            Instantiate(laser, transform.position, transform.rotation);
            shootCooldown = startShootCooldown;
        }
    }
}    


  
