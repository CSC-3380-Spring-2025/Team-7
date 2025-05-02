using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static Codice.CM.Common.CmCallContext;

public class LaserEnemy : MonoBehaviour, IDamageable
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
    private int currentHP;

    //Loottable for dropped items
    [Header("Loot")]
    public List<LootItem> LootTable = new List<LootItem>();

    void Start()
    {
        shootCooldown = startShootCooldown;
        PlayerCat = GameObject.FindGameObjectWithTag("PlayerCat");
        SetEnemyValues();
        //currentHP = data.hp;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerCat.transform.position);

        if (distanceToPlayer <= shootingRange)
        {
            // AimAtPlayer();
            Swarm();
            Shoot();
      
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
        currentHP = data.hp;
        GetComponent<Health>().SetHearts(currentHP, currentHP);
        damage = data.damage;
        speed = data.speed;
    }

    private void Shoot()
    {
        if (shootCooldown <= 0)
        {
        Vector2 direction = (PlayerCat.transform.position - transform.position).normalized;

         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        // Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction); // 2D forward = Z axis

        Instantiate(laser, transform.position, rotation);
        shootCooldown = startShootCooldown;
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        Debug.Log($"{gameObject.name} took {dmg} damage. HP: {currentHP}");

        if (currentHP <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        //Spawn Item dropped
        foreach(LootItem LootItem in LootTable) {
            if(Random.Range(0f,100f) <= LootItem.DropChance) {
                InstantiateLoot(LootItem.ItemPrefab);
            }
            break;
        }
        Destroy(gameObject);
    }

    //Uses the prefab to make the item
    void InstantiateLoot(GameObject loot) {
        if(loot){
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);
        }
    }
}    