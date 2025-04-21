using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserEnemy : MonoBehaviour
{
     private GameObject PlayerCat;
    public GameObject laser;
     [SerializeField]
    private EnemyData data;
      [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float speed = 1.5f;  
     // Radius within which the enemy detects the player
    [SerializeField]
    private float detectionRange = 5f;  
    [SerializeField] private float startShootCooldown = 2f;
    [SerializeField] private float shootingRange = 10f; // Max distance to shoot
    [SerializeField] private LayerMask obstacleLayer;  // Layer for obstacles
    [SerializeField] private float stopDistance = 2f; // How close the enemy is allowed to get


    private float shootCooldown;

    void Start()
    {
        shootCooldown = startShootCooldown;
         PlayerCat = GameObject.FindGameObjectWithTag("PlayerCat");
        SetEnemyValues();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerCat.transform.position);

        if (distanceToPlayer <= shootingRange)
        {
            // AimAtPlayer();
            Shoot();
            Swarm();
            
        }

        // Reduce cooldown timer
        if (shootCooldown > 0)
            shootCooldown -= Time.deltaTime;
    }

     private void Swarm()
{
    float distanceToPlayer = Vector2.Distance(transform.position, PlayerCat.transform.position);

    if (distanceToPlayer <= detectionRange && distanceToPlayer > stopDistance)
    {
        // Move towards the player but stop when within 'stopDistance'
        transform.position = Vector2.MoveTowards(transform.position, PlayerCat.transform.position, speed * Time.deltaTime);
        
    }
    
}


     private void SetEnemyValues()
    {
        GetComponent<Health>().SetHearts(data.hp, data.hp);
        damage = data.damage;
        speed = data.speed;
    }

    private void Shoot()
    {
        if (shootCooldown <= 0)
        {
        Vector2 direction = (PlayerCat.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction); // 2D forward = Z axis

        Instantiate(laser, transform.position, rotation);
        shootCooldown = startShootCooldown;
        }
    }
}    


  
