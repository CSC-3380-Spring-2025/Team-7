using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour {
    public GameObject attackArea1;
    public GameObject attackArea2;

//variables to check if player is attacking, set attack to last certain duration, time how long attack has been active
    private bool attacking1 = false;
    private bool attacking2 = false;
    private float timeToAttack1 = 0.25f;
    private float timetoAttack2 = 0.5f;
    private float timer1 = 0f;
    private float timer2 = 0f;

    //start is called before the first frame update
    void Start() {
        attackArea1 = transform.GetChild(0).gameObject;
        attackArea2 = transform.GetChild(1).gameObject;
    } 

    //update is called once per frame
    //If space bar is pressed, attack method is called
    void Update() {
        if(Input.GetKeyDown(KeyCode.Q)) {
            Attack1();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            Attack2();
        }

        if(attacking1) {
            timer1 += Time.deltaTime;

            if(timer1 >= timeToAttack1) {
                timer1 = 0;
                attacking1 = false;
                attackArea1.SetActive(false);
        }

        if(attacking2) {
            timer2 += Time.deltaTime; 

            if(timer2 >= timetoAttack2)
                timer2 = 0;
                attacking2 = false;
                attackArea2.SetActive(false);
        }
    }
}
//attack method sets attack area to true and attackarea checks if there are colliders in the trigger area 

    void Attack1() {
        attacking1 = true;
        attackArea1.SetActive(true);
    }

    void Attack2() {
        attacking2 = true;
        attackArea2.SetActive(true);
    }
}

}