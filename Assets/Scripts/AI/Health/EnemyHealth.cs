using CustomAttributes;
using Sodom.Enemies.AI;
using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IKillable, IEnemySensor
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private GameObject _corpseObject;
    [SerializeField] private float _corpseFlingStrength = 5f;
    [SerializeField] private bool shouldStun;
    [SerializeField, Tooltip("The amount of time after being stunned that the enemy can't be stunned again for.")] 
    private float stunCooldown;
    [SerializeField] private StunnedState stunState;

    [SerializeField, ShowIfNull] private EnemyController enemyController;

    private float currentHealth;
    private bool isStunCooldown;

    public event Action<GameObject, SenseType, bool> EntitySenseEvent;

    private void Reset()
    {
        enemyController = GetComponent<EnemyController>();
    }

    private void Start()
    {
        currentHealth = _maxHealth;
    }

    public bool OnDamage(float damageAmount, GameObject damageSource = null)
    {
        currentHealth -= damageAmount;
        EntitySenseEvent?.Invoke(damageSource, SenseType.Damage, true);

        if (shouldStun && !isStunCooldown)
        {
            // If the enemy is sucesssfully stunned, then set the stun cooldown.
            if (enemyController.SetState(stunState))
            {
                StartCoroutine(StunCooldown());
            }
        }

        if (currentHealth <= 0f)
            OnKill(damageSource);

        return currentHealth <= 0f;
    }

    private IEnumerator StunCooldown()
    {
        isStunCooldown = true;
        yield return new WaitForSeconds(stunCooldown);
        isStunCooldown = false;
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
