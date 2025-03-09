using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public Rigidbody2D Body;
    public float MoveSpeed = 2f;
    public Vector2 Direction;
    public Animator Animator;

    void Update() {
        Direction.x = Input.GetAxisRaw("Horizontal");
        Direction.y = Input.GetAxisRaw("Vertical");
        if(Direction != Vector2.zero) {
            Animator.SetFloat("Horizontal", Direction.x);
            Animator.SetFloat("Vertical", Direction.y);
        }
        Animator.SetFloat("Speed", Direction.sqrMagnitude);
       
    }   

    void FixedUpdate() {
        Body.MovePosition(Body.position + Direction * MoveSpeed * Time.fixedDeltaTime);
    }
}
