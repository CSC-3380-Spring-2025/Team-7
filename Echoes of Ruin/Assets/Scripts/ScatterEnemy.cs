using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScatterEnemy : MonoBehaviour, IDamageable
{
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
    // private int damage = 1;
    // [SerializeField]
      private float speed = 1.5f;  
    [SerializeField] private float stopDistance = 2f; // How close the enemy is allowed to get


    void Start()
    {
        shootCooldown = startShootCooldown;
        PlayerCat = GameObject.FindGameObjectWithTag("PlayerCat");
        SetEnemyValues();
        //currentHP = data.hp;
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
       if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(999);
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
        currentHP = data.hp;
        // GetComponent<Health>().SetHearts(currentHP, currentHP);
        // damage = data.damage;
        // speed = data.speed;

    }

    void ShootBullets()
    {
        float angleStep = spreadAngle / (bulletCount - 1); // Angle between bullets 
        float startAngle = -spreadAngle / 2; //Starting angle

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletAngle = startAngle + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + bulletAngle);

             Instantiate(Scatter, transform.position, transform.rotation); // spawns bulltets at enemy location 
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
        Destroy(gameObject);
    }

}

