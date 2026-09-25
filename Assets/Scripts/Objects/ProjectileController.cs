/*****************************************************************************
// File Name : ProjectileController.cs
// Author : Pierce
// Creation Date : -
// Last Modified : 6/21/2026
//
// Brief Description : Basic projectile behavior script.
*****************************************************************************/
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _projectileDamage = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IKillable ik))
        {
            ik.OnDamage(_projectileDamage, gameObject);
        }
        Destroy(gameObject);
    }
}
