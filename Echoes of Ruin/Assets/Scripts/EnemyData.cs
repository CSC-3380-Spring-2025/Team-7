using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    //Script gives you the ability to set enemy hp,damage, and speed in unity 
    public int hp;
    public int damage;
    public int speed;

}
