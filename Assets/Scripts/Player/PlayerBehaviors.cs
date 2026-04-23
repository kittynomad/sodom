using System.Collections;
using UnityEngine;

public class PlayerBehaviors : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private float _playerWalkAcceleration;
    [SerializeField] private float _playerJumpForce;
    [SerializeField] private float _playerWalkSpeedLimit;
    [SerializeField] private float _projectileSpeed;

    [SerializeField] private GameObject _hurtBox;
    [SerializeField] private SpriteRenderer _sprite;
    private SwordController sc;

    [SerializeField] private GameObject _projectile;

    private float currentHealth;
    private int currentAmmo;
    private bool isAttacking = false;

    private Rigidbody2D rb;
    private PlayerController pc;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
    public int MaxAmmo { get => _maxAmmo; set => _maxAmmo = value; }

    public bool CanFire { get => currentAmmo > 0; }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        pc = gameObject.GetComponent<PlayerController>();
        sc = _hurtBox.GetComponent<SwordController>();

        CurrentHealth = _maxHealth / 2;
        currentAmmo = MaxAmmo;
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
        if(sc.HasCorpseAttached) sc.DetachObject(pc.MovementDirection);
    }

    public void InteractBehavior()
    {
        Debug.Log("interact behgavior");
        if (sc.HasCorpseAttached && currentHealth < _maxHealth)
        {
            currentHealth += sc.AttachedObject.GetComponent<CorpseController>().HealthValue;
        }
            sc.DetachObject(pc.MovementDirection, true);
    }

    public void FireBehavior(Vector2 fireDirection)
    {
        if(CanFire)
        {
            GameObject temp = Instantiate(_projectile, transform.position, Quaternion.identity);
            temp.GetComponent<Rigidbody2D>().AddForce(fireDirection * _projectileSpeed);
            currentAmmo--;
        }
        
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
