using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeEnemy : MonoBehaviour
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

    private GameObject CatPlayer;


    //Start is called before the first frame update
    void Start()
    {

        CatPlayer = GameObject.FindGameObjectWithTag("Player");

        SetEnemyValues();
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

        float distanceToPlayer = Vector2.Distance(transform.position, CatPlayer.transform.position);
        if(distanceToPlayer <= detectionRange) {

             //Move towards the player if within detection range
        transform.position = Vector2.MoveTowards(transform.position, CatPlayer.transform.position, speed * Time.deltaTime);

        }
    }

    
    // private void OnTriggerEnter2D(Collider2D collider) 
    // {
    //     if(collider.CompareTag("Player"))
    //     {
    //         if(collider.GetComponent<Health>() != null) 
    //         {
    //             collider.GetComponent<Health>().Damage(damage);
    //             this.GetComponent<Health>().Damage(10000);
    //         }
        // }

    // }
}
