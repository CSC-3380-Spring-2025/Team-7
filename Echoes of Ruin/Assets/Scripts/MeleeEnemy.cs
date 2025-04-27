using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static Codice.CM.Common.CmCallContext;

public class MeleeEnemy : MonoBehaviour, IDamageable
{

    // Radius within which the enemy detects the player
    [SerializeField]
    private float detectionRange = 5f;
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float speed = 1.5f;
    [SerializeField]
    private EnemyData data;
    //want to make ememy follow player thats why palyer is defined

    private GameObject PlayerCat;
    private int currentHP;

    //Loottable for dropped items
    [Header("Loot")]
    public List<LootItem> LootTable = new List<LootItem>();

    //Start is called before the first frame update
    void Start()
    {

        PlayerCat = GameObject.FindGameObjectWithTag("PlayerCat");

        SetEnemyValues();
        currentHP = data.hp;
    }

    //Update is called once per frame, allows for continous upadating
    void Update()
    {
        Swarm();
    }

//value of enemy health same as max
    private void SetEnemyValues()
    {
        GetComponent<Health>().SetHearts(data.hp, data.hp);
        damage = data.damage;
        speed = data.speed;
    }

//makes object move toward player
    private void Swarm()
    {

        float distanceToPlayer = Vector2.Distance(transform.position, PlayerCat.transform.position);
        if(distanceToPlayer <= detectionRange) {

             //Move towards the player if within detection range
        transform.position = Vector2.MoveTowards(transform.position, PlayerCat.transform.position, speed * Time.deltaTime);

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
