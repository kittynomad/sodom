using System.Collections;
using UnityEngine;

public class PlayerBehaviors : MonoBehaviour
{
    [SerializeField] private float _playerWalkAcceleration;
    [SerializeField] private float _playerJumpForce;
    [SerializeField] private float _playerWalkSpeedLimit;

    [SerializeField] private GameObject _hurtBox;
    private SwordController sc;

    private bool isAttacking = false;

    private Rigidbody2D rb;
    private PlayerController pc;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        pc = gameObject.GetComponent<PlayerController>();
        sc = _hurtBox.GetComponent<SwordController>();
    }
    public void FixedUpdate()
    {
        
        if(Mathf.Abs(rb.linearVelocityX) < _playerWalkSpeedLimit)
            rb.AddForce(new Vector2(pc.MovementDirection.x * _playerWalkAcceleration, 0f));

        if (!IsAttacking) _hurtBox.transform.localPosition = pc.MovementDirection * 0.5f;
        //_hurtBox.transform.localPosition = isAttacking ? pc.MovementDirection : pc.MovementDirection * 0.5f;
        //_hurtBox.transform.localRotation = Quaternion.
    }

    public void JumpBehavior()
    {
        rb.AddForce(_playerJumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    public void AttackBehavior()
    {
        if(!IsAttacking)
        StartCoroutine(AttackCoroutine());
    }

    public void InteractBehavior()
    {
        Debug.Log("interact behgavior");
        sc.DetachObject(Vector2.up);
    }

    public bool IsGrounded()
    {
        return true;
    }

    public IEnumerator AttackCoroutine()
    {
        _hurtBox.transform.localPosition = pc.MovementDirection * 1.5f;
        IsAttacking = true;
        _hurtBox.SetActive(true);
        yield return new WaitForSeconds(1f);
        //_hurtBox.SetActive(false);
        IsAttacking = false;
    }
}
