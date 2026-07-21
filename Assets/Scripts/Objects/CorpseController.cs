using UnityEngine;

public class CorpseController : InteractableEntity
{
    [SerializeField] private float _healthValue;
    [SerializeField] private int _ammoValue;
    [SerializeField] private float _damageValue;
    [SerializeField] private GameObject _destroyParticles;
    [SerializeField] private Vector2 _flingStrength;

    private bool discarded = false;

    public float HealthValue { get => _healthValue; set => _healthValue = value; }
    public int AmmoValue { get => _ammoValue; set => _ammoValue = value; }
    public float DamageValue { get => _damageValue; set => _damageValue = value; }
    public bool Discarded { get => discarded; set => discarded = value; }
    public Vector2 FlingStrength { get => _flingStrength; set => _flingStrength = value; }

    private void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().linearVelocity = _flingStrength;
    }
    public void DeathFling()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Discarded)
        {
            if(collision.gameObject.TryGetComponent(out IKillable ik))
            {
                ik.OnDamage(_damageValue, gameObject);
            }
            DestroyCorpse();
        }
    }

    public override void OnInteract(PlayerBehaviors pb)
    {
        base.OnInteract(pb);
        try
        {
            FindAnyObjectByType<SwordController>().TryAttachCorpse(gameObject);
        }
        catch
        {
            Debug.LogWarning("unable to attach corpse, sword may not exist");
        }
    }

    public void DestroyCorpse()
    {
        Instantiate(_destroyParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
