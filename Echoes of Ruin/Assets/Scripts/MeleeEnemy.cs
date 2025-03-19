using System.Xml.Serialization;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float speed = 1.5f;
    [SerializeField]
    private EnemyData data;
    private GameObject player;

    //Start is called befpre the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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
