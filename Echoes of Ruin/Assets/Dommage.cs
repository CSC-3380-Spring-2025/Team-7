using UnityEngine;

public class Dommage : MonoBehaviour
{
    public int damage = 1;

    public enum AttackType { Melee, Range }
    public AttackType attackType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            if (attackType == AttackType.Range)
            {
                Destroy(gameObject);
            }
        }
    }
}
