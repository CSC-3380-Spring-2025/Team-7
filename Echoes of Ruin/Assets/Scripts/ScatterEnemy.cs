using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScatterEnemy : MonoBehaviour, IDamageable
{   StatsTracking tracking;
    public GameObject PlayerCat;  //refrense to player
    public GameObject Scatter; //scatter bullets
    public int bulletCount = 5; //Number of bullets per shot
    public float spreadAngle = 45f; //Total spread angle of bullets
    private float shootCooldown;
    public float startShootCooldown = 2f;
    private int currentHP;

    //enemy movemment
    private float detectionRange = 5f;  
    [SerializeField]
    private EnemyData data;
    [SerializeField]
    private int damage = 1;
    [SerializeField]
      private float speed = 1.5f;  
    [SerializeField] private float stopDistance = 2f; // How close the enemy is allowed to get

    //Loottable for dropped items
    [Header("Loot")]
    public List<LootItem> LootTable = new List<LootItem>();

    void Start()
    {
        shootCooldown = startShootCooldown;
        PlayerCat = GameObject.FindGameObjectWithTag("PlayerCat");
        tracking = GameObject.Find("HeartsAndCoinsOverlay").GetComponent<StatsTracking>();
        SetEnemyValues();
    }

    //checks if cooldown is at 0 and shoots bullets if yes, if no then timer decreases  
    void Update()
    {
      float distanceToPlayer = Vector2.Distance(transform.position, PlayerCat.transform.position);

       if (shootCooldown <= 0) 
       {
        Swarm();
        ShootBullets();
        shootCooldown = startShootCooldown;
       } else {
        shootCooldown -= Time.deltaTime;
       }
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
        if(this != null){
            currentHP = data.hp;
                if(GetComponent<Health>() != null) {
                GetComponent<Health>().SetHearts(currentHP, currentHP);
                }else{
                    return;
                }
            damage = data.damage;
            speed = data.speed;
        }
    }

    void ShootBullets()
    {
        float angleStep = spreadAngle / (bulletCount - 1); // Angle between bullets 
        float startAngle = -spreadAngle / 2; //Starting angle

        Vector2 shootDirection = (PlayerCat.transform.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = baseAngle + startAngle + (i * angleStep);
            float rad = currentAngle * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            GameObject bullet = Instantiate(Scatter, transform.position, Quaternion.Euler(0, 0, currentAngle)); // spawns bulltets at enemy location
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = direction * 10f; // Use your desired bullet speed
            }
        }
    }

    public void TakeDamage(int damage)
    {   damage  = 1 + tracking.statBonus;
        currentHP -= damage;
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

