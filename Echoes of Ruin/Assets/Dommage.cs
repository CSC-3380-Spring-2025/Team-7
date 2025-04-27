using UnityEngine;

public class Dommage : MonoBehaviour
{
    public float damage = 1;

    public enum AttackType { Melee, Range }
    public AttackType attackType;

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyData enemy = collision.GetComponent<EnemyData>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            if (attackType == AttackType.Range)
            {
            Destroy(gameObject);
            }
        }
    }*/
}
