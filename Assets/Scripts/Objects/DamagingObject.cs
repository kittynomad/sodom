using TFOOL;
using UnityEngine;

public class DamagingObject : MonoBehaviour
{
    [SerializeField] private float damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((CollisionLayer)collision.gameObject.layer == CollisionLayer.Player 
            && collision.gameObject.TryGetComponent(out PlayerBehaviors pb))
        {
            pb.OnDamage(damageAmount, gameObject);
        }
    }
}
