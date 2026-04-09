using UnityEngine;

public class CorpseController : MonoBehaviour
{
    [SerializeField] private float _healthValue;
    [SerializeField] private int _ammoValue;
    [SerializeField] private float _damageValue;
    [SerializeField] private GameObject _destroyParticles;

    private bool discarded = false;

    public float HealthValue { get => _healthValue; set => _healthValue = value; }
    public int AmmoValue { get => _ammoValue; set => _ammoValue = value; }
    public float DamageValue { get => _damageValue; set => _damageValue = value; }
    public bool Discarded { get => discarded; set => discarded = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Discarded)
        {
            if(collision.gameObject.TryGetComponent(out IKillable ik))
            {
                ik.OnDamage(_damageValue);
            }
            Instantiate(_destroyParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
            
    }
}
