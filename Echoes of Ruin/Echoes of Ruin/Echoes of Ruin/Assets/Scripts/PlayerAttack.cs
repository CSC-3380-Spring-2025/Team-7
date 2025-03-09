using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    private GameObject attackArea = default;

    private bool attacking = false;

    private float timeToAttack = 0.25f;
    private float timer = 0f;

    //start is called before the first frame update
    void Start() {
        attackArea = transfrom.GetChild(0).gameObject;
    } 

    //update is called once per frame
    //If space bar is pressed, attack method is called
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Attack();
        }

        if(attacking) {
            timer += timer.dealtaTime;

            if(timer >= timeToAttack) {
                timer = 0;
                attacking - false;
                attackArea.SetActive(attacking)
            }
        }
    }
//attack method sets attack area to true and attackarea checks if there are colliders in the trigger area 

    private void Attack() {
        attacking = true;
        attackArea.SetActive(attacking);
    }
}

