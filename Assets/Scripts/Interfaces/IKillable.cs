using UnityEngine;

public interface IKillable
{
    public bool OnDamage(float damageAmount);

    public void OnKill();
}
