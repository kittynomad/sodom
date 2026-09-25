using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class TestKillable : MonoBehaviour, IKillable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private GameObject _corpseObject;
    [SerializeField] private float _corpseFlingStrengthX = 5f;
    [SerializeField] private float _corpseFlingStrengthY = 5f;
    [SerializeField] private ParticleSystem _bloodParticleSystem;
    [SerializeField] private Material _hitFlashMat;
    private SpriteRenderer _sR;
    [SerializeField] private GameObject _bloodDeathFX;

    private float currentHealth;

    private void Start()
    {
        _sR = gameObject.GetComponent<SpriteRenderer>();
        currentHealth = _maxHealth;
    }

    public bool OnDamage(float damageAmount, GameObject damageSource = null)
    {
        currentHealth -= damageAmount;
        StartCoroutine(HitFlash());
        if (_bloodParticleSystem != null)
        {
            _bloodParticleSystem.Play();
        }
        if (currentHealth <= 0f)
        {
            OnKill(damageSource);
        }

        return currentHealth <= 0f;
    }

    public void OnKill(GameObject damageSource = null)
    {

        GameObject bloodParticle = Instantiate(_bloodParticleSystem.gameObject, transform.position, Quaternion.identity);
        GameObject bloodSmear = Instantiate(_bloodDeathFX, transform.position, Quaternion.identity);
        GameObject corpse = Instantiate(_corpseObject, transform.position, transform.rotation);

        bloodParticle.GetComponent<ParticleSystem>().Play();
        
        if(damageSource != null)
        {
            bool damageSourceIsRight = damageSource.transform.position.x > transform.position.x;
            if (damageSourceIsRight) bloodSmear.transform.localScale = new Vector3(-1, 1, 1);
            corpse.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(_corpseFlingStrengthX * (damageSourceIsRight ? -1 : 1), _corpseFlingStrengthY);
        }

        if(gameObject.TryGetComponent(out MoneyDroppingObject mdo))
        {
            mdo.DropMoney();
        }

        Destroy(gameObject);
    }
    public IEnumerator HitFlash()
    {
        Material _ogM = _sR.material;
        _sR.material = _hitFlashMat;
        _sR.color = Color.white;
        yield return new WaitForSeconds(0.15f);
        _sR.material = _ogM;
        _sR.color = Color.red;
    }
}
