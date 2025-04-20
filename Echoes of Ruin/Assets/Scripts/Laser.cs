using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;

public class Laser : MonoBehaviour
{
    //set speed value in unity
    // public float speed;
    [Range(1,10)]
    [SerializeField] 
    private float speed = 10f;
    public GameObject impactEffect;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Destroy (gameObject, lifeTime);
        
    } 

    void Update ()
    {
        // rb.linearVelocity = moveDirection * speed;
        rb.linearVelocity = transform.right * speed;
    }

    //   private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("PlayerCat"))
    //     {
    //         if (impactEffect != null)
    //         {
    //             // Optionally instantiate effect
    //             // GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
    //             // Destroy(effect, 1f);
    //         }

    //         Destroy(gameObject);
    //     }
    // }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "PlayerCat"){
            Destroy(gameObject);
        }
    }

}
