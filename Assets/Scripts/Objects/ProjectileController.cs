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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IKillable ik))
        {
            ik.OnDamage(1f, gameObject);
        }
        Destroy(gameObject);
    }
}
