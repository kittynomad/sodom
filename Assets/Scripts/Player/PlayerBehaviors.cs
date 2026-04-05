using UnityEngine;

public class PlayerBehaviors : MonoBehaviour
{
    [SerializeField] private float _playerWalkAcceleration;
    [SerializeField] private float _playerJumpForce;
    [SerializeField] private float _playerWalkSpeedLimit;

    private Rigidbody2D rb;
    private PlayerController pc;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        pc = gameObject.GetComponent<PlayerController>();
    }
    public void FixedUpdate()
    {
        
        if(Mathf.Abs(rb.linearVelocityX) < _playerWalkSpeedLimit)
            rb.AddForce(pc.MovementDirection * _playerWalkAcceleration);
    }

    public void JumpBehavior()
    {
        rb.AddForce(_playerJumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    public bool IsGrounded()
    {
        return true;
    }
}
