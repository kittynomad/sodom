using System.Collections;
using UnityEngine;

public class PlayerBehaviors : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _playerWalkAcceleration;
    [SerializeField] private float _playerJumpForce;
    [SerializeField] private float _playerWalkSpeedLimit;

    [SerializeField] private GameObject _hurtBox;
    [SerializeField] private SpriteRenderer _sprite;
    private SwordController sc;

    private int currentHealth;
    private bool isAttacking = false;

    private Rigidbody2D rb;
    private PlayerController pc;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        pc = gameObject.GetComponent<PlayerController>();
        sc = _hurtBox.GetComponent<SwordController>();

        CurrentHealth = _maxHealth;
    }
    public void FixedUpdate()
    {
        
        if(Mathf.Abs(rb.linearVelocityX) < _playerWalkSpeedLimit)
            rb.AddForce(new Vector2(pc.MovementDirection.x * _playerWalkAcceleration, 0f));

        if (!IsAttacking) _hurtBox.transform.localPosition = pc.MovementDirection * 0.5f;
        //_hurtBox.transform.localPosition = isAttacking ? pc.MovementDirection : pc.MovementDirection * 0.5f;
        //_hurtBox.transform.localRotation = Quaternion.

        FlipSpriteForVelocity();
    }

    public void FlipSpriteForVelocity()
    {
        _sprite.flipX = rb.linearVelocityX < 0f;
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
        sc.DetachObject(pc.MovementDirection);
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
        yield return new WaitForSeconds(0.5f);
        //_hurtBox.SetActive(false);
        IsAttacking = false;
    }
}
