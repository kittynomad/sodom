using System.Collections;
using UnityEngine;

public class PlayerBehaviors : MonoBehaviour
{
    [SerializeField] private float _playerWalkAcceleration;
    [SerializeField] private float _playerJumpForce;
    [SerializeField] private float _playerWalkSpeedLimit;

    [SerializeField] private GameObject _hurtBox;

    private bool isAttacking = false;

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
            rb.AddForce(new Vector2(pc.MovementDirection.x * _playerWalkAcceleration, 0f));

        //_hurtBox.transform.localPosition = isAttacking ? pc.MovementDirection : pc.MovementDirection * 0.5f;
        //_hurtBox.transform.localRotation = Quaternion.
    }

    public void JumpBehavior()
    {
        rb.AddForce(_playerJumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    public void AttackBehavior()
    {
        if(!isAttacking)
        StartCoroutine(AttackCoroutine());
    }

    public bool IsGrounded()
    {
        return true;
    }

    public IEnumerator AttackCoroutine()
    {
        _hurtBox.transform.localPosition = pc.MovementDirection;
        isAttacking = true;
        _hurtBox.SetActive(true);
        yield return new WaitForSeconds(1f);
        _hurtBox.SetActive(false);
        isAttacking = false;
    }
}
