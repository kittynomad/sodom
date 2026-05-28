using System;
using System.Collections;
using UnityEngine;

public class PlayerBehaviors : MonoBehaviour
{
    [Header("General stats")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private float _playerWalkAcceleration;
    [SerializeField] private float _playerJumpForce;
    [SerializeField] private float _playerWalkSpeedLimit;
    [SerializeField] private float _projectileSpeed;

    [Header("Ability stats")]
    [SerializeField] private float _poundStrength;
    [SerializeField] private float _poundDamage;
    [SerializeField] private float _poundPushStrength;
    [SerializeField] private float _dashStrength;

    [Header("References")]
    [SerializeField] private LayerMask _solidLayer;
    [SerializeField] private GameObject _hurtBox;
    [SerializeField] private SpriteRenderer _sprite;
    private SwordController sc;

    [SerializeField] private GameObject _projectile;

    private float currentHealth;
    private int currentAmmo;
    private bool isAttacking = false;
    private bool doubleJumpReady = true;
    private bool anchored = false;
    private bool pounding = false;

    private Rigidbody2D rb;
    private Collider2D coll;
    private PlayerController pc;
    private PlayerLoadout pl;

    public Action<PlayerBehaviors> interactAction;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
    public int MaxAmmo { get => _maxAmmo; set => _maxAmmo = value; }
    public bool CanFire { get => currentAmmo > 0; }
    public float PlayerWalkSpeedLimit { get => _playerWalkSpeedLimit; set => _playerWalkSpeedLimit = value; }
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        pc = gameObject.GetComponent<PlayerController>();
        sc = _hurtBox.GetComponent<SwordController>();
        pl = gameObject.GetComponent<PlayerLoadout>();
        coll = gameObject.GetComponent<Collider2D>();

        CurrentHealth = _maxHealth / 2;
        currentAmmo = MaxAmmo;
    }
    public void FixedUpdate()
    {
        if (pounding) rb.linearVelocity = new Vector2(rb.linearVelocityX, -1 * _poundStrength);
        if (pounding && PoundHitCheck()) PoundConnectBehavior();

        if (!anchored && Mathf.Abs(rb.linearVelocityX) < _playerWalkSpeedLimit)
            rb.AddForce(new Vector2(pc.MovementDirection.x * _playerWalkAcceleration, 0f));

        if (!IsAttacking) _hurtBox.transform.localPosition = pc.MovementDirection * 0.5f;
        //_hurtBox.transform.localPosition = isAttacking ? pc.MovementDirection : pc.MovementDirection * 0.5f;
        //_hurtBox.transform.localRotation = Quaternion.
        
        FlipSpriteForVelocity();
    }

    public void FlipSpriteForVelocity()
    {
        //_sprite.flipX = rb.linearVelocityX == 0 ? _sprite.flipX : rb.linearVelocityX < 0f;
        _sprite.flipX = pc.MovementDirection.x == 0 ? _sprite.flipX : pc.MovementDirection.x < 0f;
    }

    public void JumpBehavior()
    {
        anchored = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (IsGrounded())
        {
            rb.AddForce(_playerJumpForce * Vector2.up, ForceMode2D.Impulse);
        }
        else if(pl.DoubleJumpUnlocked && doubleJumpReady)
        {
            doubleJumpReady = false;
            rb.AddForce(_playerJumpForce * Vector2.up, ForceMode2D.Impulse);
        }
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
        if(interactAction != null)
        {
            interactAction?.Invoke(this);
        }
        else if (sc.HasCorpseAttached && currentHealth < _maxHealth)
        {
            currentHealth += sc.AttachedObject.GetComponent<CorpseController>().HealthValue;
            sc.DetachObject(pc.MovementDirection, true);
        }
            
    }

    public void FireBehavior(Vector2 fireDirection)
    {
        if (sc.HasCorpseAttached && currentAmmo < _maxAmmo)
        {
            currentAmmo += sc.AttachedObject.GetComponent<CorpseController>().AmmoValue;
            sc.DetachObject(pc.MovementDirection, true);
        }
        else if (CanFire)
        {
            GameObject temp = Instantiate(_projectile, transform.position, Quaternion.identity);
            temp.GetComponent<Rigidbody2D>().AddForce(fireDirection * _projectileSpeed);
            currentAmmo--;
        }
        
    }

    public void PoundBehavior()
    {
        if(pl.PoundUnlocked && !IsGrounded())
        {
            pounding = true;
            rb.linearVelocity = Vector2.down * _poundStrength;
        }
        else if(pl.DashUnlocked && IsGrounded())
        {
            if(pc.MovementDirection.x != 0)
            {
                rb.AddForce(Vector2.left * (pc.MovementDirection.x < 0 ? _dashStrength : _dashStrength * -1), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.left * (_sprite.flipX ? _dashStrength : _dashStrength * -1), ForceMode2D.Impulse);
            }
        }
        
    }

    public void PoundConnectBehavior()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.position += (Vector3)Vector2.down * 0.5f;
        pounding = false;
        anchored = true;
    }

    public bool PoundHitCheck()
    {
        Debug.DrawRay(transform.position, Vector2.down * 1.1f, Color.red, 1f);
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, _solidLayer);
        return hitGround;
    }

    public bool IsGrounded()
    {
        //Debug.DrawRay(transform.position, Vector2.down * 1.1f, Color.red, 1f);
        //RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, _solidLayer);
        bool hg = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, _solidLayer);
        if (hg) doubleJumpReady = true;
        return hg;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(pounding)
        {
            if(collision.gameObject.TryGetComponent<IKillable>(out IKillable ik))
            {
                ik.OnDamage(_poundDamage, gameObject);
            }
            if(collision.transform.position.x > transform.position.x)
            {
                collision.transform.position = collision.transform.position + (Vector3)Vector2.right * coll.bounds.size.x;
                collision.rigidbody.AddForceX(_poundPushStrength, ForceMode2D.Impulse);
            }
            else
            {
                collision.transform.position = collision.transform.position - (Vector3)Vector2.right * coll.bounds.size.x;
                collision.rigidbody.AddForceX(_poundPushStrength * -1, ForceMode2D.Impulse);
            }
                
        }
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
