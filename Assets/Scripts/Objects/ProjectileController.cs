using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IKillable ik))
        {
            ik.OnDamage(1f, gameObject);
            Destroy(gameObject);
        }
    }
}
