using UnityEngine;

public class StageHazard : MonoBehaviour
{
    [SerializeField] private float damageAmount = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerBehaviors pb))
        {
            pb.OnDamage(damageAmount, gameObject);
        }
        if (collision.gameObject.TryGetComponent(out GroundSaver gs))
        {
            gs.SafeReturn();
        }
    }
}
