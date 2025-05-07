using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D Body;
    public GameObject PlayerCat; // Reference to player
    public Animator Animator; // Optional, for animation

    private Vector2 direction;

    void Start()
    {
        // Assign player if not already set in inspector
        if (PlayerCat == null)
            PlayerCat = GameObject.FindGameObjectWithTag("PlayerCat");
    }

    void Update()
    {
        if (PlayerCat != null)
        {
            // Get direction to player
            direction = (PlayerCat.transform.position - transform.position).normalized;

            // Optionally, update animations if needed
            if (Animator != null)
            {
                Animator.SetFloat("Horizontal", direction.x);
                Animator.SetFloat("Vertical", direction.y);
                Animator.SetFloat("Speed", direction.sqrMagnitude);
            }
        }
    }

}
