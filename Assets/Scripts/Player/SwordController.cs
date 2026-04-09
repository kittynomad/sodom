using UnityEngine;

public class SwordController : MonoBehaviour
{
    private PlayerBehaviors pb;
    private GameObject attachedObject;
    [SerializeField] private float detachSpeed = 5f;
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
            attachedObject = collision.gameObject;
        }
    }
    public void DetachObject(Vector2 direction)
    {
        Debug.Log("gurg");
        if(attachedObject != null)
        {
            Debug.Log("grug");
            attachedObject.transform.parent = null;
            attachedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            //attachedObject.GetComponent<Rigidbody2D>().excludeLayers = LayerMask.GetMask();
            attachedObject.GetComponent<Rigidbody2D>().linearVelocity = transform.parent.GetComponent<Rigidbody2D>().linearVelocity;
            attachedObject.GetComponent<Rigidbody2D>().AddForce(direction * detachSpeed, ForceMode2D.Impulse);
            attachedObject.GetComponent<CorpseController>().Discarded = true;
        }
        attachedObject = null;
    }
}
