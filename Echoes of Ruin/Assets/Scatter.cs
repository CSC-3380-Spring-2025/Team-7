using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;

public class Scatter : MonoBehaviour
{
    //set speed value in unity
    // public float speed;
    [Range(1, 10)]
    [SerializeField]
    private float speed = 10f;

    // [Range(1,10)]
    // [SerializeField]
    // private float lifeTime = 3f;

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

    void Update()
    {
        // transform.Translate(Vector2.up * speed * Time.deltaTime);
        rb.linearVelocity = transform.right * speed;
    }
}
