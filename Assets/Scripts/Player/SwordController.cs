using UnityEngine;

public class SwordController : MonoBehaviour
{
    private PlayerBehaviors pb;
    private void Start()
    {
        pb = FindAnyObjectByType<PlayerBehaviors>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(pb.IsAttacking && collision.gameObject.TryGetComponent(out CorpseController cc))
        {
            collision.gameObject.transform.parent = gameObject.transform;
            collision.gameObject.transform.localPosition = Vector2.zero;
            collision.rigidbody.bodyType = RigidbodyType2D.Kinematic;
            collision.rigidbody.excludeLayers = LayerMask.GetMask("Player");
        }
    }
}
