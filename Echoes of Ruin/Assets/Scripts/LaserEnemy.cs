using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserEnemy : MonoBehaviour
{
    // will need to to attach a collider
    public Transform player;
    public GameObject laser;
    private float shootCooldown;
    public float startShootCooldown;

    void Start()
    {
        shootCooldown = startShootCooldown;
    }

    void Update()
    {
        Vector2 direction = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y);

        transform.up = direction;

        if(shootCooldown <=0)
        {
            Instantiate(laser, transform.position, transform.rotation);
            shootCooldown = startShootCooldown;

        } else {
            shootCooldown -= Time.deltaTime;
        }
    } 
}
