using UnityEngine;

public interface IKillable
{
    public bool OnDamage(float damageAmount, GameObject damageSource = null);

    public void OnKill(GameObject damageSource = null);
}
