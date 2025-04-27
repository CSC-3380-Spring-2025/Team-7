using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEditor.Rendering.Analytics;

public class Laser : MonoBehaviour
{
    //set speed value in unity
    // public float speed;
    [Range(1,10)]
    [SerializeField] 
    private float speed = 10f;

    [Range(1,10)]
    [SerializeField]
    private float lifeTime = 3f;
      private EnemyData data;
      [SerializeField]
    private int damage = 1;
    [SerializeField]

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Collider2D myCollider;

     void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();

        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }

        Destroy(gameObject, lifeTime);
    } 

    public void IgnoreCollider(Collider2D col){
        if(myCollider != null && col != null) {
            Physics2D.IgnoreCollision(myCollider, col);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if(collision.CompareTag ("PlayerCat"))
        {
            Health health = collision.GetComponent<Health>();
            if (health != null) 
            {
                health.Damage(damage);
                Debug.Log("Player hit, applying damage.");
            }
            
            Destroy(gameObject);
        }
        else if (!collision.isTrigger)
        {
             Destroy(gameObject);
        }
    }
}