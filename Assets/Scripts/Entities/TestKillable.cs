using UnityEngine;

public class TestKillable : MonoBehaviour, IKillable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private GameObject _corpseObject;

    private float currentHealth;

    private void Start()
    {
        currentHealth = _maxHealth;
    }

    public bool OnDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0f)
            OnKill();

        return currentHealth <= 0f;
    }

    public void OnKill()
    {
        Instantiate(_corpseObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
