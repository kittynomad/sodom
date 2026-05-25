using UnityEngine;

public class TestKillable : MonoBehaviour, IKillable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private GameObject _corpseObject;
    [SerializeField] private float _corpseFlingStrength = 5f;

    private float currentHealth;

    private void Start()
    {
        currentHealth = _maxHealth;
    }

    public bool OnDamage(float damageAmount, GameObject damageSource = null)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0f)
            OnKill(damageSource);

        return currentHealth <= 0f;
    }

    public void OnKill(GameObject damageSource = null)
    {
        GameObject corpse = Instantiate(_corpseObject, transform.position, transform.rotation);
        if(damageSource != null)
        {
            Vector2 damageDir = Vector2.Normalize(transform.position - damageSource.transform.position);
            corpse.GetComponent<Rigidbody2D>().AddForce(damageDir * _corpseFlingStrength, ForceMode2D.Impulse);
        }
        Destroy(gameObject);
    }

}
