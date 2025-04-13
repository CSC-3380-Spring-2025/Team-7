using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour {
    public GameObject MeleeAttack;
    public GameObject RangeAttack;

//variables to check if player is attacking, set attack to last certain duration, time how long attack has been active
    private bool melee = false;
    private bool range = false;
    private float timeToMelee = 0.25f;
    private float timeToRange = 0.5f;
    private float timer1 = 0f;
    private float timer2 = 0f;

//variables for range (hairball) attack
    public Transform Aimer;
    public float fireSpeed = 10f;

    //start is called before the first frame update
    void Start() {
        MeleeAttack = transform.GetChild(0).gameObject;
        RangeAttack = transform.GetChild(1).gameObject;
    }

    //update is called once per frame
    //If space bar is pressed, attack method is called
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Attack1();
            Audios.Instance.PlaySound("Woosh1");
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            Attack2();
            Audios.Instance.PlaySound("Woosh2");
        }

        if (melee)
        {
            timer1 += Time.deltaTime;

            if (timer1 >= timeToMelee)
            {
                timer1 = 0;
                melee = false;
                MeleeAttack.SetActive(false);
            }
        }
        
        if(range) {
            timer2 += Time.deltaTime; 

            if(timer2 >= timeToRange)
                timer2 = 0;
                GameObject intRange = Instantiate(RangeAttack, Aimer.position, Aimer.rotation);
                intRange.GetComponent<Rigidbody2D>().AddForce(-Aimer.up * fireSpeed, ForceMode2D.Impulse);

                range = false;
                RangeAttack.SetActive(false);
        }
    
}
//attack method sets attack area to true and attackarea checks if there are colliders in the trigger area 

    void Attack1() {
        melee = true;
        MeleeAttack.SetActive(true);
    }

    void Attack2() {
        range = true;
        RangeAttack.SetActive(true);
    }
}

