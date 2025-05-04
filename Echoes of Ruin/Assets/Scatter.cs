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
    private float speed = 3f;

    [Range(1, 10)]
    [SerializeField]
    private float lifeTime = 3f;
    private EnemyData data;
    [SerializeField]
    private int damage = 1;
    [SerializeField]

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Collider2D myCollider;

    //Timer for Health Deduction for player
    [SerializeField] private float hpDeductTime = 1f;
    private float nextHPDeductTime = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();

        Destroy(gameObject, lifeTime);
    }

    public void IgnoreCollider(Collider2D col)
    {
        if (myCollider != null && col != null)
        {
            Physics2D.IgnoreCollision(myCollider, col);
        }
    }

    //Player takes damage on collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject HeartsCoinsUI;
        HeartsCoinsUI = GameObject.FindGameObjectWithTag("HeartsCoins");

        float currentTime = Time.time;

        if (currentTime >= nextHPDeductTime)
        {
            if (collision.CompareTag("PlayerCat"))
            {
                Destroy(gameObject);
                Health health = HeartsCoinsUI.GetComponent<Health>();
                if (health != null)
                {
                    health.Damage(damage);
                    nextHPDeductTime = currentTime + hpDeductTime;
                }

                Destroy(gameObject);
            }
        }
        else if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }

    }
}
