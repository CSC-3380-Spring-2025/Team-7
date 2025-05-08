using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Vector2 lastAimDirection = Vector2.down;
    public Rigidbody2D Body;
    public float MoveSpeed = 2f;
    public Vector2 Direction;
    public Animator Animator;

    public Transform Aimer;
    bool isWalking = true;

    void Update() {
        Direction.x = Input.GetAxisRaw("Horizontal");
        Direction.y = Input.GetAxisRaw("Vertical");
        if(Direction != Vector2.zero) {
            Animator.SetFloat("Horizontal", Direction.x);
            Animator.SetFloat("Vertical", Direction.y);
            lastAimDirection = Direction.normalized;
        }

        Animator.SetFloat("Speed", Direction.sqrMagnitude);
       
    }   

    void FixedUpdate() {
        Body.MovePosition(Body.position + Direction * MoveSpeed * Time.fixedDeltaTime);
        if (isWalking)
        {
            Vector3 melee_aim = new Vector3(lastAimDirection.x, lastAimDirection.y, 0f);
            Aimer.rotation = Quaternion.LookRotation(Vector3.forward, -melee_aim);
        }
    }
}
