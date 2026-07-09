/*****************************************************************************
// File Name : PlayerBehaviors.cs
// Author : Pierce
// Creation Date : -
// Last Modified : 6/21/2026
//
// Brief Description : Handles most player-centric functions.
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;

public class PlayerBehaviors : MonoBehaviour, IKillable
{
    [Header("General stats")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private float _playerWalkAcceleration;
    [SerializeField] private float _playerJumpForce;
    [SerializeField] private float _playerWalkSpeedLimit;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _hurtRecoveryPeriod;
    [SerializeField] [Range(0, 1)] private float _customVelocityFalloffRate = 0f;
    [SerializeField][Range(0, 1)] private float _customMeleeComboVelocityFalloffRate = 0f;

    [Header("Ability stats")]
    [SerializeField] private float _poundStrength;
    [SerializeField] private float _poundDamage;
    [SerializeField] private float _poundPushStrength;
    [SerializeField] private float _dashStrength;

    [Header("References")]
    [SerializeField] private LayerMask _solidLayer;
    [SerializeField] private GameObject _hurtBox;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _anim;
    private SwordController sc;
    [SerializeField] private GameObject _projectile;

    //components
    private Rigidbody2D rb;
    private Collider2D coll;
    private PlayerController pc;
    private PlayerLoadout pl;
    private PlayerResources pr;

    //state related vars
    private float currentHealth;
    private int currentAmmo;
    private bool isAttacking = false;
    private bool doubleJumpReady = true;
    private bool anchored = false;
    private bool pounding = false;
    private bool recovering = false;
    private float moveModifier = 1f;
    private bool moveLocked = false;
    private bool meleeSliding = false;
    private bool meleeChaining = false;

    //actions
    public Action<PlayerBehaviors> interactAction;

    //getters/setters
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
    public int MaxAmmo { get => _maxAmmo; set => _maxAmmo = value; }
    public bool CanFire { get => currentAmmo > 0; }
    public float PlayerWalkSpeedLimit { get => _playerWalkSpeedLimit; set => _playerWalkSpeedLimit = value; }
    public float MoveModifier { get => moveModifier; set => moveModifier = value; }

    private void Start()
    {
        //set component references
        rb = gameObject.GetComponent<Rigidbody2D>();
        pc = gameObject.GetComponent<PlayerController>();
        sc = _hurtBox.GetComponent<SwordController>();
        pl = gameObject.GetComponent<PlayerLoadout>();
        pr = gameObject.GetComponent<PlayerResources>();
        coll = gameObject.GetComponent<Collider2D>();

        //set health and ammo
        CurrentHealth = _maxHealth / 2;
        currentAmmo = MaxAmmo;

        if(SpawnPointHolder.instance != null && SpawnPointHolder.instance.GetRelevantSpawnPoint(out Vector2 pos))
        {
            transform.position = pos;
        }
    }
    public void FixedUpdate()
    {
        if ((pc.MovementDirection.x == 0 || moveLocked) && rb.linearVelocityX != 0 && !meleeChaining)
            rb.linearVelocityX *= 1 - _customVelocityFalloffRate;
        //else if (meleeChaining)
        //{
        //
        //}
        //player walks in input direction IF not past max speed and not anchored
        else if (!anchored && Mathf.Abs(rb.linearVelocityX) < _playerWalkSpeedLimit && !moveLocked)
            rb.linearVelocityX = pc.MovementDirection.x * _playerWalkAcceleration * MoveModifier;
            //rb.AddForce(new Vector2(pc.MovementDirection.x * _playerWalkAcceleration, 0f));
        
            //set hurtBox pos while not attacking
        //if (!IsAttacking) _hurtBox.transform.localPosition = pc.MovementDirection * 0.5f;

        //ensure player keeps consistent y velocity while mid ground pound
        if (pounding) rb.linearVelocity = new Vector2(rb.linearVelocityX, -1 * _poundStrength);
        //check if time to end pound
        if (pounding && PoundHitCheck()) PoundConnectBehavior();

        FlipSpriteForVelocity();
        UpdateAnimator();
    }

    public void FlipSpriteForVelocity()
    {
        //_sprite.flipX = pc.MovementDirection.x == 0 ? _sprite.flipX : pc.MovementDirection.x < 0f;
        if(pc.MovementDirection.x != 0 && !moveLocked)
        {
            transform.localScale = new Vector3(pc.MovementDirection.x < 0f ? -1f : 1f, 1f, 1f);
        }
        
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x >= 0f;
    }

    public void JumpBehavior()
    {
        //player gets un-anchored by jumping
        anchored = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //jump if on ground
        if (!moveLocked)
        {
            if (IsGrounded())
            {
                rb.AddForce(_playerJumpForce * Vector2.up, ForceMode2D.Impulse);
            }
            //also jump if not grounded but have double jump still
            else if (pl.DoubleJumpUnlocked && doubleJumpReady)
            {
                doubleJumpReady = false;
                rb.AddForce(_playerJumpForce * Vector2.up, ForceMode2D.Impulse);
            }
        }
    }

    public void AttackBehavior()
    {
        //can't start attack if already attacking
        //if(!IsAttacking)
        StartCoroutine(AttackCoroutine());
        //launch attached corpse on attack 
        if(sc.HasCorpseAttached) sc.DetachObject(pc.MovementDirection);
    }

    public void InteractBehavior()
    {
        Debug.Log("interact behgavior");
        //broadcast interactAction if it has subscribers
        if(interactAction != null)
        {
            interactAction?.Invoke(this);
        }
        //eat attached corpse on interact if no interactibles around
        else if (sc.HasCorpseAttached && currentHealth < _maxHealth)
        {
            currentHealth += sc.AttachedObject.GetComponent<CorpseController>().HealthValue;
            sc.DetachObject(pc.MovementDirection, true);
        }
            
    }

    public float FacingDirection()
    {
        return IsFacingRight() ? -1f : 1f;   
    }

    public void FireBehavior(Vector2 fireDirection, bool mouseAim = false)
    {
        //turn attached corpse into ammo if below max ammo
        if (sc.HasCorpseAttached && currentAmmo < _maxAmmo)
        {
            currentAmmo += sc.AttachedObject.GetComponent<CorpseController>().AmmoValue;
            sc.DetachObject(pc.MovementDirection, true);
        }
        //fire projectile otherwise
        else if (CanFire)
        {
            GameObject temp = Instantiate(_projectile, transform.position, Quaternion.identity);
            //fire towards mouse pos if mouseAim set to true
            if (mouseAim)
            {
                temp.GetComponent<Rigidbody2D>().AddForce(fireDirection * _projectileSpeed);
            }
            //fire in input direction otherwise
            else
            {
                Vector2 fd;
                if(pc.MovementDirection.y == 0)
                {
                    fd = FacingDirection() * Vector2.right;
                }
                else
                {
                    fd =  new Vector2(0.7f * FacingDirection(), 0.7f * pc.MovementDirection.y);
                }
                temp.GetComponent<Rigidbody2D>().AddForce(fd * _projectileSpeed);
            }
            
            currentAmmo--;
        }
        
    }

    public void PoundBehavior()
    {
        //if not grounded, do a vertical ground pound
        if(pl.PoundUnlocked && !IsGrounded() && !moveLocked)
        {
            pounding = true;
            rb.linearVelocity = Vector2.down * _poundStrength;
        }
        //if grounded, do a horizontal dash
        else if(pl.DashUnlocked && IsGrounded() && !moveLocked)
        {
            //dash in input direction if there is current directional input
            if(pc.MovementDirection.x != 0)
            {
                rb.AddForce(Vector2.left * (pc.MovementDirection.x < 0 ? _dashStrength : _dashStrength * -1), ForceMode2D.Impulse);
            }
            //if no directional input, dash in direction player is facing
            else
            {
                rb.AddForce(Vector2.left * (IsFacingRight() ? _dashStrength : _dashStrength * -1), ForceMode2D.Impulse);
            }
        }
        
    }

    public void PoundConnectBehavior()
    {
        //go from pound state to anchored state
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.position += (Vector3)Vector2.down * 0.5f;
        pounding = false;
        anchored = true;
    }

    public bool PoundHitCheck()
    {
        //check if time to end ground pound
        bool hg = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, _solidLayer);
        return hg;
    }

    public bool IsGrounded()
    {
        //check if grounded (duh)
        bool hg = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, _solidLayer);
        if (hg) doubleJumpReady = true;
        return hg;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //behavior for pushing objects out of the way while ground pounding
        if(pounding)
        {
            //damage entity if it implements IKillable
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

    public void CollectCurrency(float value)
    {
        pr.Currency += value;
    }

    public IEnumerator AttackCoroutine()
    {
        _anim.SetBool("AttackBuffered", true);
        print("attack buffered");
        yield return null;
        //if (IsGrounded())
        //{
        //    _anim.SetBool("AttackBuffered", true);
        //}
        //else
        //{
        //    _hurtBox.transform.localPosition = pc.MovementDirection * 1.5f;
        //    StartAttack();
        //    yield return new WaitForSeconds(0.5f);
        //    EndAttack();
        //}
    }

    public void StartAttack()
    {
        IsAttacking = true;
        _hurtBox.SetActive(true);
    }

    public void EndAttack()
    {
        IsAttacking = false;
        //_hurtBox.SetActive(false);
    }

    public IEnumerator HurtRecoveryCoroutine()
    {
        recovering = true;
        yield return new WaitForSeconds(_hurtRecoveryPeriod);
        recovering = false;
    }

    public bool OnDamage(float damageAmount, GameObject damageSource = null)
    {
        if(!recovering)
        {
            currentHealth -= damageAmount;
            //bounce away from damage source
            Vector2 damageDir = Vector2.Normalize(transform.position - damageSource.transform.position);
            rb.linearVelocity = damageDir * 5f;
            StartCoroutine(HurtRecoveryCoroutine());
        }

        return false;

    }

    public void OnKill(GameObject damageSource = null)
    {
        throw new NotImplementedException();
    }

    public void UpdateAnimator()
    {
        Vector2 relativeVelocity;
        if ((transform.parent != null && transform.parent.TryGetComponent(out Rigidbody2D prb)))
        {
            //Debug.Log(prb.linearVelocity);
            relativeVelocity = rb.linearVelocity - prb.linearVelocity;
        }
        else relativeVelocity = rb.linearVelocity;
        _anim.SetBool("IsGrounded", IsGrounded());
        _anim.SetBool("IsMoving", (pc.MovementDirection.x != 0));
        _anim.SetFloat("YSpeed", relativeVelocity.y);
        _anim.SetFloat("XSpeed", relativeVelocity.x);
    }

    public void MoveLockOn()
    {
        moveLocked = true;
    }
    public void MoveLockOff()
    {
        moveLocked = false;
    }
}
